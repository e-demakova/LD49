namespace Deblue.ObservingSystem
{
    public readonly struct PropertyChanged<T>
    {
        public readonly T NewValue;
        public readonly T OldValue;

        public PropertyChanged(T newValue, T oldValue)
        {
            NewValue = newValue;
            OldValue = oldValue;
        }
    }

    public readonly struct PropertySet<T>
    {
        public readonly T Value;

        public PropertySet(T value)
        {
            Value = value;
        }
    }

    public readonly struct LimitedPropertyChanged<T>
    {
        public readonly T NewValue;

        public readonly T LowerLimit;
        public readonly T UpperLimit;

        public LimitedPropertyChanged(T newValue, T lowerLimit, T upperLimit)
        {
            NewValue = newValue;

            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
        }
    }

    public readonly struct PropertyReachedLowerLimit
    {
    }

    public readonly struct PropertyReachedUpperLimit
    {
    }

    public readonly struct ValueChanged<TKey, TValue>
    {
        public readonly TKey Key;
        public readonly TValue OldValue;
        public readonly TValue NewValue;

        public ValueChanged(TKey key, TValue oldValue, TValue newValue)
        {
            Key = key;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public readonly struct ValueRemoved<TKey, TValue>
    {
        public readonly TKey Index;
        public readonly TValue Value;

        public ValueRemoved(TKey key, TValue value)
        {
            Index = key;
            Value = value;
        }
    }

    public readonly struct ValueAdded<TKey, TValue>
    {
        public readonly TKey Key;
        public readonly TValue Value;

        public ValueAdded(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
