using Deblue.ObservingSystem;
using UnityEngine;

namespace Deblue.InteractiveObjects
{
    public enum InteractExecutionOrder
    {
        First,
        Second,
        Third
    }
    public interface IInteractItem
    {
        InteractExecutionOrder Order { get; }
        bool CanHighlight(IItemTaker taker);
        void Interact(IItemTaker taker);
        void Highlight();
        void StopHighlight();
    }

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class InteractItem : MonoBehaviour, IInteractItem
    {
        [System.Serializable]
        protected struct SpritePair
        {
            public Sprite Standard;
            public Sprite Highlight;
        }

        [SerializeField] protected string _id;

        protected readonly Handler<InteractObjectUpdated> _updated = new Handler<InteractObjectUpdated>();
        protected bool _isTaken;

        protected SpriteRenderer Renderer { get; private set; }
        public IReadOnlyHandler<InteractObjectUpdated> Updated => _updated;

        public abstract InteractExecutionOrder Order { get; }

        protected void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            MyAwake();
        }

        protected virtual void MyAwake()
        {
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<IItemTaker>(out var taker))
                return;

            _updated.Raise(new InteractObjectUpdated(this));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent<IItemTaker>(out var taker))
                return;

            _updated.Raise(new InteractObjectUpdated(this));
        }

        public abstract bool CanHighlight(IItemTaker taker);
        public abstract void Interact(IItemTaker taker);
        public abstract void Highlight();
        public abstract void StopHighlight();
    }
}