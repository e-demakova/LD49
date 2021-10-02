using Deblue.UI.Views;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace Deblue.Achivments
{
    public class AchivmentViewPresenter :  ModelViewPresenter<AchivmentModel>
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _description;

        private static readonly int UnlockTrigger = Animator.StringToHash("Unlock");

        protected override void MyInit()
        {
        }

        public override void Dispose()
        {
        }

        private void ShowAchivment(AchivmentUnlock context)
        {
            _label.text = context.Label;
            _description.text = context.Description;
            _icon.sprite = context.Icon;
            _animator.SetTrigger(UnlockTrigger);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }

    public readonly struct AchivmentUnlock
    {
        public readonly string Label;
        public readonly string Description;
        public readonly Sprite Icon;

        public AchivmentUnlock(AchivmentsTable achivments, string id)
        {
            var achivment = achivments[id];
            Label = achivment.Label;
            Description = achivment.Description;
            Icon = achivment.Icon;
        }
    }
}
