using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Deblue.Data.Localization;
using Deblue.ObservingSystem;
using Deblue.Story.Characters;
using Deblue.Story.DialogSystem.Choices;
using Deblue.UI.Views;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Deblue.Story.DialogSystem
{
    [Serializable]
    public struct UIElement
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector2 _openPosition;
        [SerializeField] private Vector2 _closePosition;

        private Image _image;

        public Image Image => _image ??= _rectTransform.GetComponent<Image>();
        public RectTransform RectTransform => _rectTransform;
        public Vector3 OpenPosition => LocalToWorld(_openPosition);
        public Vector3 ClosePosition => LocalToWorld(_closePosition);

        private Vector2 LocalToWorld(Vector2 position)
        {
            var anchor = _rectTransform.position - (Vector3) _rectTransform.anchoredPosition;
            return anchor + (Vector3) position;
        }
    }

    public class AutoScaledDialogView: ModelViewPresenter<DialogSwitcher>
    {
        public override void Hide()
        {
            throw new NotImplementedException();
        }

        public override void Show()
        {
            throw new NotImplementedException();
        }

        protected override void MyInit()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
    
    public class DialogView : ModelViewPresenter<DialogSwitcher>
    {
        [Header("Animation settings")]
        [SerializeField] private bool _snapping;
        [SerializeField] private float _animationTime;

        [Header("PopUp setup")]
        [SerializeField] private TextMeshProUGUI _replica;
        [FormerlySerializedAs("_PopUpPositions")] [SerializeField]
        private UIElement _PopUp;

        [Header("Choices setup")]
        [SerializeField] private ChoiceView _choicePrefab;
        [SerializeField] private Transform _choicesContainer;

        [Header("Characters setup")]
        [SerializeField] private CharactersDataSO _charactersData;
        [FormerlySerializedAs("_leftPositions")] [SerializeField]
        private UIElement _leftCharacter;
        [FormerlySerializedAs("_rightPositions")] [SerializeField]
        private UIElement _rightCharacter;

        private readonly List<ChoiceView> _choices = new List<ChoiceView>(4);
        private readonly List<IObserver> _observers = new List<IObserver>(4);
        private LocalizationService _localization;

        [Inject]
        public void Construct(LocalizationService localization)
        {
            _localization = localization;
            _charactersData.SetCharactersToDictionary();
        }

        protected override void MyInit()
        {
            Model.Events.ReplicaSwitched.Subscribe(VisualizeNewReplica, _observers);
            Model.Events.DialogStarted.Subscribe(x => ShowElement(_PopUp), _observers);
            Model.Events.DialogEnded.Subscribe(x => HideElements(), _observers);
            Model.Events.DialogGaveChoice.Subscribe(VisualizeChoice, _observers);
        }

        public override void Dispose()
        {
            _observers.ClearObservers();
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        private void VisualizeNewReplica(ReplicaSwitched context)
        {
            SetTextToPopUp(context);
            VisualizeCharacter(context.Replica.CharacterId);
        }

        private void SetTextToPopUp(ReplicaSwitched context)
        {
            _replica.text = _localization.GetText(context.Replica.TextId);
        }

        private void VisualizeCharacter(string id)
        {
            var character = _charactersData.GetCharacter(id);
            var isPlayer = character.IsPlayer;

            var showedCharacter = isPlayer ? _leftCharacter : _rightCharacter;
            var hiddenCharacter = isPlayer ? _rightCharacter : _leftCharacter;

            showedCharacter.Image.sprite = character.Sprite;
            ShowElement(showedCharacter);
            HideElement(hiddenCharacter);
        }

        private void ShowElement(UIElement element)
        {
            element.RectTransform.DOMove(element.OpenPosition, _animationTime, _snapping);
        }

        private void HideElements()
        {
            HideElement(_PopUp);
            HideElement(_leftCharacter);
            HideElement(_rightCharacter);
        }

        private void HideElement(UIElement element)
        {
            element.RectTransform.DOMove(element.ClosePosition, _animationTime, _snapping);
        }

        private void VisualizeChoice(DialogGaveChoice context)
        {
            _replica.text = _localization.GetText(context.ChoiceTextId);

            while (_choices.Count < context.Choices.Length)
            {
                InstantiateChoice();
            }

            for (int i = 0; i < _choices.Count; i++)
            {
                _choices[i].SetChoice(context.Choices[i]);
                _choices[i].Show();
            }
        }

        private void InstantiateChoice()
        {
            var newChoice = Instantiate(_choicePrefab, _choicesContainer);
            _choices.Add(newChoice);

            newChoice.ChoiceMade.Subscribe(x =>
            {
                Model.OnChoiceMade(x.Choice);
                HideChoices();
            }, _observers);
        }

        private void HideChoices()
        {
            for (int i = 0; i < _choices.Count; i++)
            {
                _choices[i].Hide();
            }
        }
    }
}