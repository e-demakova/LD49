using System;
using Deblue.ObservingSystem;
using TMPro;
using UnityEngine;

namespace Deblue.Stats.View
{
    public class TextStatView<TEnum> : StatView<TEnum> where TEnum : System.Enum
    {
        [SerializeField] private TextMeshProUGUI _statText;

        public override void UpdateView(LimitedPropertyChanged<float> context)
        {
            _statText.text = Math.Round(context.NewValue).ToString();
        }

        protected override void Init()
        {
            _statText.text = 0.ToString();
        }
    }
}