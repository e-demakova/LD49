namespace Deblue.InteractiveObjects
{
    public abstract class ReactionItem : IInteractItem
    {
        public InteractExecutionOrder Order { get; } = InteractExecutionOrder.Second;

        public void Interact(IItemTaker taker)
        {
            React(taker);
        }

        protected abstract void React(IItemTaker taker);
        public abstract bool CanHighlight(IItemTaker taker);
        public abstract void Highlight();
        public abstract void StopHighlight();
    }
}