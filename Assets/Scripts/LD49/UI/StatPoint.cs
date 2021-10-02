using UnityEngine;
using UnityEngine.UI;

namespace LD49.UI
{
    public class StatPoint : MonoBehaviour
    {
        [SerializeField] private Sprite _fillSprite;
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private Image _renderer;

        private bool _isFill;
        
        public bool IsFill
        {
            get => _isFill;
            set
            {
                _isFill = value;
                _renderer.sprite = value ? _fillSprite : _emptySprite;
            }
        } 
    }
}