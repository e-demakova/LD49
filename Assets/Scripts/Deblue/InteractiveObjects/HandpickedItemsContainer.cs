using UnityEngine;

namespace Deblue.InteractiveObjects
{
    public abstract class HandpickedItemsContainer : InteractItem
    {
        [SerializeField] protected string _containedObjectId;
        protected abstract bool CanReturn { get; }

        public override void Interact(IItemTaker taker)
        {
            if (CanReturn && taker.IsContainObject(_containedObjectId))
                ReceiveItem(taker);
            else if (taker.IsCanTakeObject && CanTake(taker))
                GiveItem();
        }

        private void ReceiveItem(IItemTaker taker)
        {
            var obj = taker.ReturnObject();
            Return(obj);
            obj.Put();
        }

        protected abstract bool CanTake(IItemTaker taker);
        protected abstract void GiveItem();
        protected abstract void Return(HandpickedItem obj);
    }
}