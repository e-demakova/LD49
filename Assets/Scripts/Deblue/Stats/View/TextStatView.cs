using Deblue.ObservingSystem;
using TMPro;
using UnityEngine;

namespace Deblue.Stats.View
{
    public class TextStatView<TEnum> : StatView<TEnum> where TEnum : System.Enum
    {
        [SerializeField] private TextMeshProUGUI _textMesh;

        protected override void Init()
        {
            _textMesh.text = Stat.Value.ToString();
        }

        public override void UpdateView(LimitedPropertyChanged<float> context)
        {
            _textMesh.text = context.NewValue.ToString();
        }
    }
}