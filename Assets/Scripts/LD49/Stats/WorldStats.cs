using System;
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

            sceneLoader.SceneLoaded.Subscribe(ChangeStable);
        }

        private void ChangeStable(SceneLoaded context)
        {
            switch (context.NewScene.Type)
            {
                case SceneType.Level:
                    _worldStats.ChangeAmount(WorldStatId.Stable, -StableDelta);
                    break;
                case SceneType.Shop:
                    RefreshStats();
                    break;
                case SceneType.UI:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RefreshStats()
        {
            _worldStats.ChangePercent(WorldStatId.Stable, 1f);
            _heroStats.ChangePercent(HeroStatId.Hp, 1f);
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