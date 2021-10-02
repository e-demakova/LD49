using Deblue.InteractiveObjects;
using UnityEngine;

namespace LD49.Stats
{
    public class StatBranch : ReactionItem
    {
        [SerializeField] private StatLief[] _liefs;
        private int _liefsActivated;

        private StatLief AvailableLief => _liefs[_liefsActivated];
        
        protected override void React(IItemTaker taker)
        {
            AvailableLief.Activate();
            StopHighlight();
            _liefsActivated++;
            Highlight();
        }

        public override bool CanHighlight(IItemTaker taker)
        {
            return _liefsActivated < _liefs.Length;
        }
        
        public override void Highlight()
        {
            AvailableLief.Highlight();
        }

        public override void StopHighlight()
        {
            AvailableLief.StopHighlight();
        }
    }
}
