using System;
using Deblue.ObservingSystem;

namespace Deblue.Stats
{
    public interface IStatsStorage
    {
        void DeInit();
    }

    public interface IReadonlyStatsStorage<TEnum> : IStatsStorage, IDisposable where TEnum : Enum
    {
        IReadonlyObservList<TEnum> StatsIds { get; }

        float GetStatValue(TEnum id);
        IReadonlyObservLimitProperty<float> GetStatProperty(TEnum id);
    }

    public interface IStatsStorage<TEnum> : IReadonlyStatsStorage<TEnum> where TEnum : Enum
    {
        void AddStat(TEnum id, float amount = 0);
        void AddStat(TEnum id, float amount, float lowerLimit, float upperLimit = float.MaxValue);
        void ChangeAmount(TEnum id, float delta);
        void SetAmount(TEnum id, float amount);
    }

    public interface ILimitedStatsStorage<TEnum> : IStatsStorage<TEnum> where TEnum : Enum
    {
        void SetUpperLimit(TEnum id, float amount);
    }
    
    public interface IModifiableStatsStorage<TEnum> : IStatsStorage<TEnum> where TEnum : Enum
    {
        void AddModifier(IStatModifier<TEnum> modifier);
        void RemoveModifier(IStatModifier<TEnum> modifier);
    }
}