using LD49.Hero;
using LD49.Stats;
using UnityEngine;
using Zenject;

namespace LD49.Enviroment
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Trap : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float _unstable;
        
        private WorldStats _stats;
        
        [Inject]
        public void Construct(WorldStats stats)
        {
            _stats = stats;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<HeroController>(out var hero)) 
                return;
            
            if (Random.value > _stats.Stable - _unstable) 
                ActivateTrap(hero);
        }

        protected abstract void ActivateTrap(HeroController hero);
    }
}
