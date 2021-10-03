using System;
using Deblue.SceneManagement;
using Deblue.Stats;
using LD49.Hero;
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

        private int _maxHpLeafs;
        private int _stableDeltaLeafs;

        public WorldStats(LimitedStatsStorage<HeroStatId> heroStats, LimitedStatsStorage<WorldStatId> worldStats, SceneLoader sceneLoader)
        {
            _heroStats = heroStats;
            _worldStats = worldStats;

            sceneLoader.SceneLoaded.Subscribe(ChangeStable);
        }

        public int GetActivatedLeafs(WorldStatId id)
        {
            var value = 0;
            switch (id)
            {
                case WorldStatId.StableDelta:
                    value = _stableDeltaLeafs;
                    break;

                case WorldStatId.MaxHp:
                    value = _maxHpLeafs;
                    break;

                default:
                    Debug.LogWarning($"Stat id {id} didn't define.");
                    break;
            }

            return value;
        }

        public void SetActivatedLeafs(WorldStatId id, int value)
        {
            switch (id)
            {
                case WorldStatId.StableDelta:
                    _stableDeltaLeafs = value;
                    break;

                case WorldStatId.MaxHp:
                    _maxHpLeafs = value;
                    break;

                default:
                    Debug.LogWarning($"Stat id {id} didn't define.");
                    break;
            }
        }

        private void ChangeStable(SceneLoaded context)
        {
            switch (context.NewScene.Type)
            {
                case SceneType.Level:
                    _worldStats.ChangeAmount(WorldStatId.Score, 10);
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

            float score = _worldStats.GetStatValue(WorldStatId.Score);
            float record = _worldStats.GetStatValue(WorldStatId.Record);

            if (score > record)
                _worldStats.SetAmount(WorldStatId.Record, score);

            _worldStats.SetAmount(WorldStatId.Score, 0f);
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