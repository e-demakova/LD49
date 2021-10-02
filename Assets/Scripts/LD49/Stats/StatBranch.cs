using Deblue.InteractiveObjects;
using UnityEngine;

namespace LD49.Stats
{
    public class StatBranch : InteractionItem
    {
        [SerializeField] private StatLeaf[] _leafs;
        private int _liefsActivated;

        private StatLeaf AvailableLeaf => _leafs[_liefsActivated];

        public override void Interact()
        {
            AvailableLeaf.Activate();
            StopHighlight();
            _liefsActivated++;
            Highlight();
        }

        public override bool CanHighlight => _liefsActivated < _leafs.Length;
        

        public override void Highlight()
        {
            if (CanHighlight)
                AvailableLeaf.Highlight();
        }

        public override void StopHighlight()
        {
            if (CanHighlight)
                AvailableLeaf.StopHighlight();
        }
    }
}