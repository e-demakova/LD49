using Deblue.ObservingSystem;
using Deblue.Stats.View;
using LD49.Stats;
using TMPro;
using UnityEngine;

namespace LD49.UI
{
    public class CoinsView : StatView<HeroStatId>
    {
        [SerializeField] private TextMeshProUGUI _coinsCount;

        public override void UpdateView(LimitedPropertyChanged<float> context)
        {
            _coinsCount.text = ((int) context.NewValue).ToString();
        }

        protected override void Init()
        {
            _coinsCount.text = 0.ToString();
        }
    }
}
