using Deblue.Stats;
using Deblue.UI;
using LD49.Stats;
using UnityEngine;
using Zenject;

namespace LD49.Installers
{
    public class UISceneInstaller : MonoInstaller<UISceneInstaller>
    {
        [SerializeField] private UIVisualizator _uiVisualizator;
        
        private LimitedStatsStorage<HeroStatId> _heroStatsStorage;
        private LimitedStatsStorage<WorldStatId> _worldStatsStorage;

        [Inject]
        public void Construct(LimitedStatsStorage<HeroStatId> heroStats, LimitedStatsStorage<WorldStatId> worldStats)
        {
            _heroStatsStorage = heroStats;
            _worldStatsStorage = worldStats;
        }

        public override void InstallBindings()
        {
            _uiVisualizator.InitView(_heroStatsStorage);
            _uiVisualizator.InitView(_worldStatsStorage);
        }
    }
}