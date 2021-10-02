using UnityEngine;

namespace Deblue.InteractiveObjects
{
    public sealed class StandardHandpickedItem : HandpickedItem
    {
        public bool CanPut => _isTaken;
        public bool IsCanBeTaken => !_isTaken;
        public override InteractExecutionOrder Order { get; } = InteractExecutionOrder.First;

        [SerializeField] private SpritePair _sprites;


        public sealed override void StopHighlight()
        {
            Renderer.sprite = _sprites.Standard;
        }

        public sealed override void Highlight()
        {
            Renderer.sprite = _sprites.Highlight;
        }

        public override bool CanHighlight(IItemTaker taker)
        {
            return !_isTaken;
        }
    }
}