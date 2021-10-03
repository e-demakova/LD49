using LD49.Hero;
using UnityEngine;
using Zenject;

namespace LD49
{
    public class ColliderSceneChanger : MonoBehaviour
    {
        private ScenesPool _scenesPool;

        [Inject]
        public void Construct(ScenesPool pool)
        {
            _scenesPool = pool;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<HeroController>(out var hero)) 
                _scenesPool.LoadNextLevel();
        }
    }
}
