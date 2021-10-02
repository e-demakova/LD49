using System;
using System.Collections;
using System.Collections.Generic;

namespace Deblue.ObservingSystem
{
    public interface IReadonlyObservList<T> : IDisposable
    {
        int Count { get; }
        int Capacity { get; }
        public T this[int i] { get; }

        IReadOnlyHandler<ValueAdded<int, T>> ValueAdded { get; }
        IReadOnlyHandler<ValueRemoved<int, T>> ValueRemoved { get; }
        IReadOnlyHandler<ValueChanged<int, T>> ValueChanged { get; }
    }

    [Serializable]
    public class ObservList<T> : IReadonlyObservList<T>, IList<T>
    {
        private Handler<ValueAdded<int, T>> _valueAdded = new Handler<ValueAdded<int, T>>();
        private Handler<ValueRemoved<int, T>> _valueRemoved = new Handler<ValueRemoved<int, T>>();
        private Handler<ValueChanged<int, T>> _valueChanged = new Handler<ValueChanged<int, T>>();

        public IReadOnlyHandler<ValueAdded<int, T>> ValueAdded => _valueAdded;
        public IReadOnlyHandler<ValueRemoved<int, T>> ValueRemoved => _valueRemoved;
        public IReadOnlyHandler<ValueChanged<int, T>> ValueChanged => _valueChanged;


        public int Count => _list.Count;
        public int Capacity => _list.Capacity;
        public bool IsReadOnly => ((ICollection<T>) _list).IsReadOnly;

        private List<T> _list;

        public ObservList() : this(0)
        {
        }

        public ObservList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        public T this[int i]
        {
            get => _list[i];
            set
            {
                _valueChanged.Raise(new ValueChanged<int, T>(i, _list[i], value));
                _list[i] = value;
            }
        }

        public void Add(T item)
        {
            _list.Add(item);
            _valueAdded.Raise(new ValueAdded<int, T>(_list.Count, item));
        }

        public void Insert(int index, T item)
        {
            Insert(index, item);
            _valueAdded.Raise(new ValueAdded<int, T>(index, item));
        }

        public void Remove(T item)
        {
            var index = _list.IndexOf(item);
            _list.Remove(item);
            _valueRemoved.Raise(new ValueRemoved<int, T>(index, item));
        }

        public void RemoveAt(int index)
        {
            var item = _list[index];
            _list.RemoveAt(index);
            _valueRemoved.Raise(new ValueRemoved<int, T>(index, item));
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            return _list.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void Clear()
        {
            for (int i = 0; i < _list.Count; i++)
            {
                Remove(_list[i]);
            }

            _valueAdded.Clear();
            _valueChanged.Clear();
            _valueRemoved.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }
}