using Deblue.Story.DialogSystem;
using UnityEngine;
using Zenject;

namespace Deblue.Story.Steps
{
    [CreateAssetMenu(fileName = "ChangeStepCondition_StartDialog", menuName = "Story/Change Step Conditions/Start Dialog")]
    public class ChangeStepOnDialogSO : ChangeStepConditionSO
    {
        [SerializeField] private string _targetCharacterId;

        private DialogSwitcher _switcher;

        [Inject]
        public void Construct(DialogSwitcher switcher)
        {
            _switcher = switcher;
        }

        protected override void MyInit()
        {
            _switcher.Events.DialogStarted.Subscribe(OnDone);
        }

        protected override void MyDeInit()
        {
            _switcher.Events.DialogStarted.Unsubscribe(OnDone);
        }

        private void OnDone(DialogStarted context)
        {
            if (context.CharacterId == _targetCharacterId) 
                IsDone = true;
        }
    }
}