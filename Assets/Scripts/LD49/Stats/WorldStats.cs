using Deblue.SceneManagement;
using Deblue.Stats;
using JetBrains.Annotations;
using UnityEngine;

namespace LD49.Stats
{
    public class WorldStats
    {
        private readonly LimitedStatsStorage<HeroStatId> _heroStats;
        private readonly LimitedStatsStorage<WorldStatId> _worldStats;
        
        public float MaxHp { get; private set; }
        private float StableDelta { get; set; } = 0.3f;
        public float Stable => _worldStats.GetStatValue(WorldStatId.Stable);

        public WorldStats(LimitedStatsStorage<HeroStatId> heroStats, LimitedStatsStorage<WorldStatId> worldStats, SceneLoader sceneLoader)
        {
            _heroStats = heroStats;
            _worldStats = worldStats;

            sceneLoader.SceneLoaded.Subscribe(DecreaseStable);
        }

        private void DecreaseStable(SceneLoaded context)
        {
            if (context.NewScene.Type == SceneType.Level)
                DecreaseStable();
        }

        private void DecreaseStable()
        {
            _worldStats.ChangeAmount(WorldStatId.Stable, -StableDelta);
        }

        public void ChangeStat(WorldStatId id, float newValue)
        {
            switch (id)
            {
                case WorldStatId.StableDelta:
                    StableDelta = newValue;
                    break;

                case WorldStatId.MaxHp:
                    MaxHp = newValue;
                    _heroStats.SetUpperLimit(HeroStatId.Hp, newValue);
                    break;

                default:
                    Debug.LogWarning($"Stat id {id} didn't define.");
                    break;
            }
        }
    }
}