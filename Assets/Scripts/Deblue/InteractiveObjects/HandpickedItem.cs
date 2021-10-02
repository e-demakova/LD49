namespace Deblue.InteractiveObjects
{
    public abstract class HandpickedItem : InteractItem
    {
        public override InteractExecutionOrder Order => InteractExecutionOrder.First;

        public override void Interact(IItemTaker taker)
        {
            if (taker.IsCanTakeObject)
                Take();
        }

        private void Take()
        {
            _isTaken = true;
            _updated.Raise(new InteractObjectUpdated(this));
        }

        public void Put()
        {
            _isTaken = false;
            _updated.Raise(new InteractObjectUpdated(this));
        }
    }
}