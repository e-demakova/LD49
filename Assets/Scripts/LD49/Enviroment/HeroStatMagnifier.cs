using Deblue.Stats;
using LD49.Hero;
using LD49.Stats;
using UnityEngine;
using Zenject;

namespace LD49.Enviroment
{
    [RequireComponent(typeof(Collider2D))]
    public class HeroStatMagnifier : MonoBehaviour
    {
        [SerializeField] private HeroStatId _id;
        private LimitedStatsStorage<HeroStatId> _stats;

        [Inject]
        public void Construct(LimitedStatsStorage<HeroStatId> stats)
        {
            _stats = stats;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<HeroController>(out var hero))
                _stats.ChangeAmount(_id, 1);
        }
    }
}