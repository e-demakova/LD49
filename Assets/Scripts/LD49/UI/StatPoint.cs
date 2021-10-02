using UnityEngine;
using UnityEngine.UI;

namespace LD49.UI
{
    [RequireComponent(typeof(Image))]
    public class StatPoint : MonoBehaviour
    {
        [SerializeField] private Color _fillColor;
        [SerializeField] private Color _emptyColor;

        private Image _renderer;
        private bool _isFill;
        
        public bool IsFill
        {
            get => _isFill;
            set
            {
                _isFill = value;
                _renderer.color = value ? _fillColor : _emptyColor;
            }
        } 
        
        private void Awake()
        {
            _renderer = GetComponent<Image>();
        }
    }
}