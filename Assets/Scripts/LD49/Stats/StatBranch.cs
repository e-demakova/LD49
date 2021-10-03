using System;
using Deblue.InteractiveObjects;
using Deblue.Stats;
using TMPro;
using UnityEngine;
using Zenject;

namespace LD49.Stats
{
    public class StatBranch : InteractionItem
    {
        [SerializeField] private WorldStatId _id;
        [SerializeField] private StatLeaf[] _leafs;
        [SerializeField] private TextMeshProUGUI _nextValue;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private AudioSource _buySound;
        [SerializeField] private AudioSource _denySound;

        private int _liefsActivated;
        private IStatsStorage<HeroStatId> _storage;
        private WorldStats _worldStats;

        private StatLeaf AvailableLeaf => _leafs[_liefsActivated];

        [Inject]
        public void Construct(IStatsStorage<HeroStatId> storage, WorldStats worldStats)
        {
            _storage = storage;
            _worldStats = worldStats;
            _liefsActivated = worldStats.GetActivatedLeafs(_id);
        }

        protected override void MyAwake()
        {
            for (int i = 0; i < _liefsActivated; i++)
            {
                _leafs[i].SetActiveView();
            }
            UpdateText();
        }

        public override void Interact()
        {
            float money = _storage.GetStatValue(HeroStatId.Money);


            bool canBuy = Math.Round(money) >= AvailableLeaf.Cost;
            var sound = canBuy ? _buySound : _denySound;
            sound.Play();
                
            if (!canBuy)
                return;

            _storage.ChangeAmount(HeroStatId.Money, -AvailableLeaf.Cost);
            AvailableLeaf.Activate();
            
            StopHighlight();
            _liefsActivated++;
            _worldStats.SetActivatedLeafs(_id, _liefsActivated);
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