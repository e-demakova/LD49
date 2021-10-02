using Deblue.ObservingSystem;
using Deblue.Story.DialogSystem;
using UnityEngine;
using Zenject;

namespace Deblue.Story.Steps
{
    [CreateAssetMenu(fileName = "ChangeStepCondition_DialogEnd", menuName = "Story/Change Step Conditions/Dialog End")]
    public class ChangeStepOnDialogEndSO : ChangeStepConditionSO
    {
        [SerializeField] private string _targetCharacterId;

        private IObserver _dialogEndObserver;
        private DialogSwitcher _switcher;

        [Inject]
        public void Construct(DialogSwitcher switcher)
        {
            _switcher = switcher;
        }

        protected override void MyInit()
        {
            _dialogEndObserver = _switcher.Events.DialogEnded.Subscribe(context =>
            {
                if (context.CharacterId == _targetCharacterId) 
                    IsDone = true;
            });
        }

        protected override void MyDeInit()
        {
            _dialogEndObserver?.Dispose();
        }
    }
}