using Deblue.ObservingSystem;
using UnityEngine;

namespace Deblue.InteractiveObjects
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class InteractionItem : MonoBehaviour
    {
        [System.Serializable]
        public struct SpritePair
        {
            public Sprite Standard;
            public Sprite Highlight;
        }
        
        public abstract bool CanHighlight { get; }

        protected void Awake()
        {
            MyAwake();
        }

        protected virtual void MyAwake()
        {
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IItemTaker>(out var taker))
            {
                taker.NearInteractionItem = this;
                Highlight();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<IItemTaker>(out var taker))
            {
                taker.NearInteractionItem = null;
                StopHighlight();
            }
        }

        public abstract void Interact();
        public abstract void Highlight();
        public abstract void StopHighlight();
    }
}