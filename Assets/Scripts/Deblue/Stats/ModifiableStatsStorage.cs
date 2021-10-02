using System;
using System.Collections.Generic;

using Deblue.ObservingSystem;

namespace Deblue.Stats
{
    public enum ApplicationOrder
    {
        First,
        Second
    }

    public interface IStatModifier
    {
        ApplicationOrder Order { get; }
        float ApplyModifier(float stat);
    }

    public interface IStatModifier<TEnum> : IStatModifier where TEnum : Enum
    {
        TEnum Id { get; }
    }

    public abstract class StatModifier : IStatModifier
    {
        public ApplicationOrder Order { get; set; }

        public abstract float ApplyModifier(float stat);
    }

    public class AdditionStatModifier<TEnum> : StatModifier, IStatModifier<TEnum> where TEnum : Enum
    {
        public float AddedValue;

        public TEnum Id { get; private set; }

        public AdditionStatModifier(TEnum id, float addedValue = 0, ApplicationOrder order = ApplicationOrder.First)
        {
            id = Id;
            AddedValue = addedValue;
            Order = order;
        }

        public override float ApplyModifier(float stat)
        {
            return stat + AddedValue;
        }
    }

    public class MultipliengStatModifier<TEnum> : StatModifier, IStatModifier<TEnum> where TEnum : Enum
    {
        public float AddedValue;

        public TEnum Id { get; private set; }

        public MultipliengStatModifier(TEnum id, float addedValue = 0, ApplicationOrder order = ApplicationOrder.Second)
        {
            id = Id;
            AddedValue = addedValue;
            Order = order;
        }

        public override float ApplyModifier(float stat)
        {
            return stat * AddedValue;
        }
    }

    public class ModifiableStatsStorage<TEnum> : BaseStatsStorage<TEnum>, IModifiableStatsStorage<TEnum> where TEnum : Enum
    {
        private readonly Dictionary<TEnum, float> _clearStats = new Dictionary<TEnum, float>(15);
        private readonly Dictionary<TEnum, HashSet<IStatModifier<TEnum>>> _firstOrderModifiers = new Dictionary<TEnum, HashSet<IStatModifier<TEnum>>>(15);
        private readonly Dictionary<TEnum, HashSet<IStatModifier<TEnum>>> _secondOrderModifiers = new Dictionary<TEnum, HashSet<IStatModifier<TEnum>>>(15);

        public override void ChangeAmount(TEnum id, float delta)
        {
            _clearStats[id] += delta;
            RefreshModifiers(id);
        }

        public override void SetAmount(TEnum id, float amount)
        {
            _clearStats[id] = amount;
            RefreshModifiers(id);
        }

        public void AddModifier(IStatModifier<TEnum> modifier)
        {
            var modifiersDict = modifier.Order == ApplicationOrder.First ? _firstOrderModifiers : _secondOrderModifiers;
            var modifiers = modifiersDict[modifier.Id];
            modifiers.Add(modifier);
            RefreshModifiers(modifier.Id);
        }

        public void RemoveModifier(IStatModifier<TEnum> modifier)
        {
            var modifiersDict = modifier.Order == ApplicationOrder.First ? _firstOrderModifiers : _secondOrderModifiers;
            var modifiers = modifiersDict[modifier.Id];
            modifiers.Remove(modifier);
            RefreshModifiers(modifier.Id);
        }

        public IStatModifier<TEnum>[] GetModifiers(TEnum id)
        {
            if (_firstOrderModifiers.TryGetValue(id, out var firstMod) &&
                _secondOrderModifiers.TryGetValue(id, out var secondMod))
            {
                var modifiers = new List<IStatModifier<TEnum>>(firstMod.Count + secondMod.Count);
                modifiers.AddRange(firstMod);
                modifiers.AddRange(secondMod);
                return modifiers.ToArray();
            }
            throw new ArgumentOutOfRangeException($"Trying to get modifiers for non-existent stat {id}."); ;
        }

        protected override void MyAddStatToEnumerators(TEnum id, ObservFloat stat)
        {
            _clearStats.Add(id, stat);
            _firstOrderModifiers.Add(id, new HashSet<IStatModifier<TEnum>>());
            _secondOrderModifiers.Add(id, new HashSet<IStatModifier<TEnum>>());
        }

        private void RefreshModifiers(TEnum id)
        {
            var statValue = _clearStats[id];
            statValue = ApplyModifiers(_firstOrderModifiers[id], statValue);
            statValue = ApplyModifiers(_secondOrderModifiers[id], statValue);
            GetStat(id).Value = statValue;
        }

        private float ApplyModifiers(HashSet<IStatModifier<TEnum>> modifiers, float value)
        {
            foreach (var modifier in modifiers)
            {
                value = modifier.ApplyModifier(value);
            }
            return value;
        }
    }
}