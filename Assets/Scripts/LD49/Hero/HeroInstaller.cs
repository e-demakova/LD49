using Deblue.ColliderFinders;
using DSlacker.ColliderFinders;
using LD49.Enviroment;
using LD49.Hero;
using LD49.Stats;
using UnityEngine;
using Zenject;

namespace LD49
{
    [RequireComponent(typeof(HeroController))]
    public class HeroInstaller : MonoBehaviour
    {
        [SerializeField] private Animator _heroAnimator;
        [SerializeField] private HeroConfigSO _heroConfig;
        
        private HeroBindings? _heroBindings;
        private HeroController _hero;
        private WorldStats _worldStats;

        private HeroBindings HeroBindings => _heroBindings ??= CreateHeroBindings();

        [Inject]
        public void Construct(WorldStats worldStats)
        {
            _worldStats = worldStats;
            
            _hero = GetComponent<HeroController>();
            _hero.Init(HeroBindings);
        }

        private HeroBindings CreateHeroBindings()
        {
            var collider = GetComponent<Collider2D>();
            var bindings = new HeroBindings()
            {
                View = new HeroView(_heroAnimator),
                Model = new HeroModel(_heroConfig),
                FloorChecker = CreateFloorChecker(collider),
                WallsChecker = CreateWallsChecker(collider),
            };
            bindings.Model.MaxHp = _worldStats.MaxHp;
            return bindings;
        }
        
        private IGroundChecker CreateFloorChecker(Collider2D collider)
        {
            return new RayGroundChecker(collider.transform, Vector2.down, collider.bounds.extents.y + 0.02f);
        }

        private IGroundChecker CreateWallsChecker(Collider2D collider)
        {
            return new WallsChecker(collider);
        }
    }

    public struct HeroBindings
    {
        public HeroModel Model;
        public HeroView View;
        public IGroundChecker FloorChecker;
        public IGroundChecker WallsChecker;
    }
}