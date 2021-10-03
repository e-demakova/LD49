using System;
using Deblue.InteractiveObjects;
using Deblue.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LD49.Stats
{
    public class StatBranch : InteractionItem
    {
        [SerializeField] private StatLeaf[] _leafs;
        [SerializeField] private TextMeshProUGUI _nextValue;
        [SerializeField] private TextMeshProUGUI _cost;

        private int _liefsActivated;
        private IStatsStorage<HeroStatId> _storage;

        private StatLeaf AvailableLeaf => _leafs[_liefsActivated];

        [Inject]
        public void Construct(IStatsStorage<HeroStatId> storage)
        {
            _storage = storage;
            UpdateText();
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

            UpdateText();
        }

        private void UpdateText()
        {
            _cost.text = AvailableLeaf.Cost.ToString();
            _nextValue.text = AvailableLeaf.NextValue;
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