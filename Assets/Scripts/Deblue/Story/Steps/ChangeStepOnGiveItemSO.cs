using Deblue.Story.Characters;
using Deblue.Story.DialogSystem;
using UnityEngine;
using Zenject;

namespace Deblue.Story.Steps
{
    [CreateAssetMenu(fileName = "ChangeStepCondition_GiveItem", menuName = "Story/Change Step Conditions/Give Item")]
    public class ChangeStepOnGiveItemSO : ChangeStepConditionSO
    {
        [SerializeField] private string _targetCharacterId;
        [SerializeField] private string _itemId;

        private DialogSwitcher _switcher;

        [Inject]
        public void Construct(DialogSwitcher switcher)
        {
            _switcher = switcher;
        }

        protected override void MyInit()
        {
            _switcher.Events.PlayerGaveItem.Subscribe(CheckGivenItem);
        }

        protected override void MyDeInit()
        {
            _switcher.Events.PlayerGaveItem.Unsubscribe(CheckGivenItem);
        }

        private void CheckGivenItem(PlayerGaveItem context)
        {
            if (context.CharacterId == _targetCharacterId && context.ItemId == _itemId) 
                IsDone = true;
        }
    }
}