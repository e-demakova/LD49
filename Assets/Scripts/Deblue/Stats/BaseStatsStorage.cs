using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Deblue.ObservingSystem;
using Deblue.Models;

namespace Deblue.Stats
{
    public abstract class BaseStatsStorage<TEnum> : IReadonlyStatsStorage<TEnum>, IModel where TEnum : Enum
    {
        public IReadonlyObservList<TEnum> StatsIds => _statsIds;

        private readonly ObservList<TEnum> _statsIds = new ObservList<TEnum>(5);

        private readonly Dictionary<TEnum, ObservFloat> _stats = new Dictionary<TEnum, ObservFloat>(15);

        public abstract void ChangeAmount(TEnum id, float delta);

        public abstract void SetAmount(TEnum id, float amount);

        public IReadonlyObservLimitProperty<float> GetStatProperty(TEnum id)
        {
            return _stats[id];
        }

        public float GetStatValue(TEnum id)
        {
            return _stats[id];
        }

        public void SetStats()
        {
            var ids = Enum.GetValues(typeof(TEnum));
            for (int i = 0; i < ids.Length; i++)
            {
                var id = (TEnum) ids.GetValue(i);
                AddStat(id, 0);
            }
        }
        
        public void SetStatsValues(Stat<TEnum>[] stats)
        {
            for (int i = 0; i < stats.Length; i++)
            {
                var stat = stats[i];
                SetAmount(stat.Id, stat.Value);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddStat(TEnum id, float amount = 0)
        {
            AddStat(id, amount, float.MinValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddStat(TEnum id, float amount, float lowerLimit, float upperLimit = float.MaxValue)
        {
            if (_stats.ContainsKey(id))
            {
                throw new Exception($"Trying adding already existing stat {id}.");
            }
            var stat = new ObservFloat(amount, lowerLimit, upperLimit);
            AddStatToEnumerators(id, stat);
        }

        private void AddStatToEnumerators(TEnum id, ObservFloat stat)
        {
            _statsIds.Add(id);
            _stats.Add(id, stat);
            MyAddStatToEnumerators(id, stat);
        }

        protected virtual void MyAddStatToEnumerators(TEnum id, ObservFloat stat)
        {
        }

        public bool TryGetStat(TEnum id, out IReadonlyObservLimitProperty<float> readOnlyStat)
        {
            var flag = _stats.TryGetValue(id, out var stat);
            readOnlyStat = stat;
            return flag;
        }

        public bool TryGetStat(TEnum id, out float floatStat)
        {
            var flag = _stats.TryGetValue(id, out var stat);
            floatStat = stat;
            return flag;
        }

        public void DeInit()
        {
            _statsIds.Clear();
            foreach (var stat in _stats)
            {
                stat.Value.Dispose();
            }
        }

        protected ObservFloat GetStat(TEnum id)
        {
            return _stats[id];
        }

        public void Dispose()
        {
            _statsIds.Dispose();
            foreach (var stat in _stats)
            {
                stat.Value.Dispose();
            }
            MyDispose();
        }

        protected virtual void MyDispose()
        {
        }
    }

    public class StatsStorage<TEnum> : BaseStatsStorage<TEnum> where TEnum : Enum
    {
        public override void ChangeAmount(TEnum id, float delta)
        {
            GetStat(id).Value += delta;
        }

        public override void SetAmount(TEnum id, float amount)
        {
            GetStat(id).Value = amount;
        }
    }
}