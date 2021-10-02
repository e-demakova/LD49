using System;
using Deblue.InteractiveObjects;
using LD49.Enviroment;
using UnityEngine;
using Zenject;

namespace LD49.Stats
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class StatLief : MonoBehaviour
    {
        [SerializeField] private StatsIds _id;
        [SerializeField] private float _newValue;
        [SerializeField] private InteractItem.SpritePair _sprites;
        
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
            _renderer.sprite = _sprites.Standard;
        }
        
        public void Activate()
        {
            _worldStats.ChangeStat(_id, _newValue);
        }
    }
}
