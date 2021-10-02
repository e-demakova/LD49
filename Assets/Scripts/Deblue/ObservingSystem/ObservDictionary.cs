using System;
using System.Collections;
using System.Collections.Generic;

namespace Deblue.ObservingSystem
{
    public interface IReadonlyObservDictionary<TKey, TValue>
    {
        IReadOnlyHandler<ValueAdded<TKey, TValue>> ValueAdded { get; }
        IReadOnlyHandler<ValueRemoved<TKey, TValue>> ValueRemoved { get; }
        IReadOnlyHandler<ValueChanged<TKey, TValue>> ValueChanged { get; }
    }

    [Serializable]
    public class ObservDictionary<TKey, TValue> : IReadonlyObservDictionary<TKey, TValue>, IDictionary<TKey, TValue>
    {
        private Handler<ValueAdded<TKey, TValue>> _valueAdded = new Handler<ValueAdded<TKey, TValue>>();
        private Handler<ValueRemoved<TKey, TValue>> _valueRemoved = new Handler<ValueRemoved<TKey, TValue>>();
        private Handler<ValueChanged<TKey, TValue>> _valueChanged = new Handler<ValueChanged<TKey, TValue>>();

        public IReadOnlyHandler<ValueAdded<TKey, TValue>> ValueAdded => _valueAdded;
        public IReadOnlyHandler<ValueRemoved<TKey, TValue>> ValueRemoved => _valueRemoved;
        public IReadOnlyHandler<ValueChanged<TKey, TValue>> ValueChanged => _valueChanged;

        public ObservDictionary() : this(0)
        {
        }

        public ObservDictionary(int capacity)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        private Dictionary<TKey, TValue> _dictionary;

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                var oldValue = _dictionary[key];
                _dictionary[key] = value;
                _valueChanged.Raise(new ValueChanged<TKey, TValue>(key, oldValue, value));
            }
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            _valueAdded.Raise(new ValueAdded<TKey, TValue>(key, value));
        }

        public bool Remove(TKey key)
        {
            var value = _dictionary[key];
            _dictionary.Remove(key);
            _valueRemoved.Raise(new ValueRemoved<TKey, TValue>(key, value));
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public void Clear()
        {
            foreach (var item in _dictionary)
            {
                _valueRemoved.Raise(new ValueRemoved<TKey, TValue>(item.Key, item.Value));
            }

            _dictionary.Clear();
            _valueAdded.Clear();
            _valueChanged.Clear();
            _valueRemoved.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.ContainsKey(item.Key) && _dictionary.ContainsValue(item.Value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Array.Resize(ref array, arrayIndex + _dictionary.Count);

            int i = arrayIndex;
            foreach (var item in _dictionary)
            {
                array[i] = item;
                i++;
            }
        }
    }
}