using System.Collections;
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
        [SerializeField] private SpriteRenderer _glitchSprite;
        
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

            bool isUnstable = Random.value > _stats.Stable - _unstable;
            
            if (isUnstable) 
                ActivateTrap(hero);
            
            StartCoroutine(Glitching(isUnstable ? Color.red : Color.black));
        }

        protected abstract void ActivateTrap(HeroController hero);

        private IEnumerator Glitching(Color color)
        {
            if (_glitchSprite == null)
                yield break;
            
            _glitchSprite.color = color;
            _glitchSprite.enabled = true;
            
            yield return new WaitForSecondsRealtime(0.3f);
            
            _glitchSprite.enabled = false;
        }
    }
}
