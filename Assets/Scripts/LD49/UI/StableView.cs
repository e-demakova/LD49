using Deblue.ObservingSystem;
using Deblue.Stats.View;
using LD49.Stats;
using TMPro;
using UnityEngine;

namespace LD49.UI
{
    public class StableView : StatView<WorldStatId>
    {
        [SerializeField] private TextMeshProUGUI _stableValue;

        public override void UpdateView(LimitedPropertyChanged<float> context)
        {
            float contextNewValue = 10 * context.NewValue;
            _stableValue.text = ((int) contextNewValue * 10).ToString();
        }

        protected override void Init()
        {
            _stableValue.text = 100.ToString();
        }
    }
}