using Deblue.InteractiveObjects;
using UnityEngine;
using Zenject;

namespace LD49.Stats
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class StatLeaf : MonoBehaviour
    {
        [SerializeField] private WorldStatId _id;
        [SerializeField] private float _newValue;
        [SerializeField] private InteractionItem.SpritePair _sprites;
        [SerializeField] private Sprite _activateSprite;

        private WorldStats _worldStats;
        private SpriteRenderer _renderer;

        [Inject]
        public void Construct(WorldStats worldStats)
        {
            _worldStats = worldStats;
        }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void Highlight()
        {
            _renderer.sprite = _sprites.Highlight;
        }

        public void StopHighlight()
        {
            if (_renderer.sprite != _activateSprite)
                _renderer.sprite = _sprites.Standard;
        }

        public void Activate()
        {
            _worldStats.ChangeStat(_id, _newValue);
            _renderer.sprite = _activateSprite;
        }
    }
}