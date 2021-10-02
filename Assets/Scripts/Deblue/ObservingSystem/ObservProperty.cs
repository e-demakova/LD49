using System;

namespace Deblue.ObservingSystem
{
    public interface IReadonlyObservLimitProperty<T> where T : IComparable, IComparable<T>, IEquatable<T>
    {
        T Value { get; }
        T LowerLimit { get; }
        T UpperLimit { get; }

        IReadOnlyHandler<LimitedPropertyChanged<T>> PropertyChanged { get; }
        IReadOnlyHandler<PropertyReachedUpperLimit> ReachedUpperLimit { get; }
        IReadOnlyHandler<PropertyReachedLowerLimit> ReachedLowerLimit { get; }
    }

    public interface IReadonlyObservProperty<T> where T : IEquatable<T>
    {
        T Value { get; }
        IReadOnlyHandler<PropertyChanged<T>> PropertyChanged { get; }
        IReadOnlyHandler<PropertySet<T>> PropertySetted { get; }
    }

    public abstract class BaseObservProperty<T> : IDisposable where T : IEquatable<T>
    {
        public abstract T Value { get; set; }

        public override string ToString() => Value.ToString();

        public override bool Equals(object obj)
        {
            return obj is BaseObservProperty<T> property && Value.Equals(property.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public abstract void Dispose();
    }

    public class ObservProperty<T> : BaseObservProperty<T>, IReadonlyObservProperty<T> where T : IEquatable<T>
    {
        private T _value;

        private readonly Handler<PropertyChanged<T>> _propertyChanged = new Handler<PropertyChanged<T>>();
        private readonly Handler<PropertySet<T>> _propertySetted = new Handler<PropertySet<T>>();

        public IReadOnlyHandler<PropertyChanged<T>> PropertyChanged => _propertyChanged;
        public IReadOnlyHandler<PropertySet<T>> PropertySetted => _propertySetted;

        public ObservProperty()
        {
        }

        public ObservProperty(T value)
        {
            Value = value;
        }

        public sealed override T Value
        {
            get => _value;
            set
            {
                if (!value.Equals(_value))
                {
                    var oldValue = _value;
                    _value = value;
                    _propertyChanged.Raise(new PropertyChanged<T>(_value, oldValue));
                }

                _propertySetted.Raise(new PropertySet<T>(_value));
            }
        }

        public override void Dispose()
        {
            _propertyChanged.Clear();
            _propertySetted.Clear();
        }
    }

    [Serializable]
    public class ObservLimitProperty<T> : BaseObservProperty<T>, IReadonlyObservLimitProperty<T> where T : IComparable, IComparable<T>, IEquatable<T>
    {
        private T _lowerLimit;
        private T _upperLimit;
        private T _value;

        private readonly Handler<LimitedPropertyChanged<T>> _propertyChanged = new Handler<LimitedPropertyChanged<T>>();
        private readonly Handler<PropertyReachedUpperLimit> _reachedUpperLimit = new Handler<PropertyReachedUpperLimit>();
        private readonly Handler<PropertyReachedLowerLimit> _reachedLowerLimit = new Handler<PropertyReachedLowerLimit>();

        public IReadOnlyHandler<LimitedPropertyChanged<T>> PropertyChanged => _propertyChanged;
        public IReadOnlyHandler<PropertyReachedUpperLimit> ReachedUpperLimit => _reachedUpperLimit;
        public IReadOnlyHandler<PropertyReachedLowerLimit> ReachedLowerLimit => _reachedLowerLimit;

        public ObservLimitProperty(T loverLimit, T upperLimit) : this(default(T), loverLimit, upperLimit)
        {
        }

        public ObservLimitProperty(T value, T loverLimit, T upperLimit)
        {
            _value = value;
            _lowerLimit = loverLimit;
            _upperLimit = upperLimit;
        }

        public sealed override T Value
        {
            get => _value;
            set
            {
                if (value.CompareTo(_lowerLimit) <= 0)
                {
                    _value = _lowerLimit;
                    _reachedLowerLimit.Raise(new PropertyReachedLowerLimit());
                }
                else if (value.CompareTo(_upperLimit) >= 0)
                {
                    _value = _upperLimit;
                    _reachedUpperLimit.Raise(new PropertyReachedUpperLimit());
                }
                else
                {
                    _value = value;
                }

                _propertyChanged.Raise(new LimitedPropertyChanged<T>(_value, _lowerLimit, _upperLimit));
            }
        }

        public override void Dispose()
        {
            _propertyChanged.Clear();
            _reachedLowerLimit.Clear();
            _reachedUpperLimit.Clear();
        }

        public T LowerLimit
        {
            get => _lowerLimit;
            set
            {
                _lowerLimit = value;
                _propertyChanged.Raise(new LimitedPropertyChanged<T>(_value, _lowerLimit, _upperLimit));
            }
        }

        public T UpperLimit
        {
            get => _upperLimit;
            set
            {
                _upperLimit = value;
                _propertyChanged.Raise(new LimitedPropertyChanged<T>(_value, _lowerLimit, _upperLimit));
            }
        }
    }
}