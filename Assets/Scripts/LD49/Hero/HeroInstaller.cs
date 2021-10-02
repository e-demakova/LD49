using Deblue.ColliderFinders;
using DSlacker.ColliderFinders;
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

        private HeroBindings HeroBindings => _heroBindings ??= CreateHeroBindings();

        [Inject]
        public void Construct()
        {
            _hero = GetComponent<HeroController>();

            _hero.Init(HeroBindings);
        }

        private HeroBindings CreateHeroBindings()
        {
            var collider = GetComponent<Collider2D>();
            return new HeroBindings()
            {
                View = new HeroView(_heroAnimator),
                Model = new HeroModel(_heroConfig),
                FloorChecker = CreateFloorChecker(collider),
                WallsChecker = CreateWallsChecker(collider),
            };
        }
        
        private IGroundChecker CreateFloorChecker(Collider2D collider)
        {
            return new RayGroundChecker(collider.transform, Vector2.down, collider.bounds.extents.y + 0.08f);
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