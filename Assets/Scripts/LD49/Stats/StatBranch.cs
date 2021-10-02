using System;
using Deblue.InteractiveObjects;
using Deblue.Stats;
using UnityEngine;
using Zenject;

namespace LD49.Stats
{
    public class StatBranch : InteractionItem
    {
        [SerializeField] private StatLeaf[] _leafs;

        private int _liefsActivated;

        private IStatsStorage<HeroStatId> _storage;

        private StatLeaf AvailableLeaf => _leafs[_liefsActivated];

        [Inject]
        public void Construct(IStatsStorage<HeroStatId> storage)
        {
            _storage = storage;
        }

        public override void Interact()
        {
            float money = _storage.GetStatValue(HeroStatId.Money);
            if (Math.Round(money) < AvailableLeaf.Cost)
                return;

            _storage.ChangeAmount(HeroStatId.Money, -AvailableLeaf.Cost);
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