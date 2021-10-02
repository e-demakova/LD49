using UnityEngine;
using Zenject;

namespace LD49.Enviroment
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Trap : MonoBehaviour
    {
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
            
            if (Random.value > _stats.Stable) 
                ActivateTrap();
        }

        protected abstract void ActivateTrap();
    }
}
