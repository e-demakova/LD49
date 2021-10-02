using Deblue.Data.Localization;
using Deblue.ObservingSystem;
using Deblue.UI.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Deblue.Story.DialogSystem.Choices
{
    public class ChoiceView : UIView
    {
        public Choice Choice;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;

        [Header("Text colors")]
        [SerializeField] private Color _enabled;

        [SerializeField] private Color _disabled;

        private readonly Handler<DialogChoiceMade> _choiceMade = new Handler<DialogChoiceMade>();
        private LocalizationService _localization;

        public IReadOnlyHandler<DialogChoiceMade> ChoiceMade => _choiceMade;

        [Inject]
        public void Constructor(LocalizationService localization)
        {
            _localization = localization;
        }

        public void SetChoice(Choice choice)
        {
            Choice = choice;
            _text.text = _localization.GetText(choice.IsAvailable ? choice.TextId : choice.BlockedTextId);

            EnableButton(choice.IsAvailable);
        }

        private void EnableButton(bool enabled)
        {
            _button.interactable = enabled;
            _text.color = enabled ? _enabled : _disabled;
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnChoiceMade()
        {
            _choiceMade.Raise(new DialogChoiceMade(Choice));
        }
    }
}