using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Deblue.Extensions;
using Deblue.ObservingSystem;
using UnityEngine;

namespace Deblue.Stats
{
    public class LimitedStatsStorage<TEnum> : BaseStatsStorage<TEnum>, ILimitedStatsStorage<TEnum>
        where TEnum : Enum
    {
        private readonly Dictionary<TEnum, float> _statPercents = new Dictionary<TEnum, float>(15);
        private readonly List<IObserver> _observers = new List<IObserver>(10);

        public float GetStatPercent(TEnum id)
        {
            return _statPercents[id];
        }

        public override void ChangeAmount(TEnum id, float delta)
        {
            var stat = GetStat(id);
            SetAmount(id, stat.Value + delta, stat);
        }

        public override void SetAmount(TEnum id, float amount)
        {
            var stat = GetStat(id);
            SetAmount(id, amount, stat);
        }

        public void ChangePercent(TEnum id, float delta)
        {
            SetPercent(id, _statPercents[id] + delta);
        }

        public void SetPercent(TEnum id, float amount)
        {
            amount = Mathf.Clamp01(amount);
            _statPercents[id] = amount;
            UpdateValue(id);
        }

        public void SetUpperLimit(TEnum id, float amount)
        {
            var stat = GetStat(id);
            SetUpperLimit(id, stat, amount);
        }

        public void ChangeUpperLimit(TEnum id, float delta)
        {
            var stat = GetStat(id);
            SetUpperLimit(id, stat, stat.Value + delta);
        }

        public void SubscribeUpperLimit(IReadonlyObservLimitProperty<float> maxStatValue, TEnum statId)
        {
            maxStatValue.PropertyChanged.Subscribe(context => SetUpperLimit(statId, context.NewValue), _observers);
            SetUpperLimit(statId, maxStatValue.Value);
        }

        protected override void MyAddStatToEnumerators(TEnum id, ObservFloat stat)
        {
            stat.LowerLimit = 0f;
            _statPercents.Add(id, CalculatePercent(stat));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetAmount(TEnum id, float amount, ObservFloat stat)
        {
            var percent = amount.CalculatePercent(stat.UpperLimit);
            SetPercent(id, percent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetUpperLimit(TEnum id, ObservFloat stat, float amount)
        {
            stat.UpperLimit = amount;
            UpdateValue(stat, id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float CalculatePercent(TEnum id)
        {
            return CalculatePercent(GetStat(id));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float CalculatePercent(ObservFloat stat)
        {
            return stat.Value.CalculatePercent(stat.UpperLimit);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float CalculateValue(TEnum id)
        {
            return CalculateValue(GetStat(id), _statPercents[id]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float CalculateValue(ObservFloat stat, float percent)
        {
            return stat.UpperLimit * percent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateValue(TEnum id)
        {
            UpdateValue(GetStat(id), id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateValue(ObservFloat stat, TEnum id)
        {
            stat.Value = CalculateValue(stat, _statPercents[id]);
        }

        protected override void MyDispose()
        {
            _observers.ClearObservers();
        }
    }
}