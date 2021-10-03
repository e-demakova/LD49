using Deblue.Data;
using Deblue.Input;
using Deblue.SceneManagement;
using Deblue.Stats;
using LD49.Stats;
using UnityEngine;
using Zenject;

namespace LD49.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField] private StartScenesConfigSO _scenes;

        public override void InstallBindings()
        {
            InstallServices();
            InstallStats();
        }

        private void InstallServices()
        {
            Container.BindInterfacesAndSelfTo<InputReceiver>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<LoadService>().FromInstance(new LoadService()).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ScenesPool>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<StartScenesConfigSO>().FromInstance(_scenes).AsSingle().NonLazy();
        }

        private void InstallStats()
        {
            var heroStats = new LimitedStatsStorage<HeroStatId>();
            heroStats.AddStat(HeroStatId.Hp, 1f, 0f, 1f);
            heroStats.AddStat(HeroStatId.Money);

            var worldStats = new LimitedStatsStorage<WorldStatId>();
            worldStats.AddStat(WorldStatId.Stable, 9f, 0f, 1f);
            worldStats.AddStat(WorldStatId.Score, 0f, 0f);
            worldStats.AddStat(WorldStatId.Record, 0f, 0f);

            Container.BindInterfacesAndSelfTo<LimitedStatsStorage<HeroStatId>>().FromInstance(heroStats).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LimitedStatsStorage<WorldStatId>>().FromInstance(worldStats).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<WorldStats>().AsSingle().NonLazy();
        }
    }
}