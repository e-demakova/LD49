using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deblue.ObservingSystem
{
    public interface IHandlerContainer<T> : IReadOnlyHandler<T>
    {
        void Clear();
    }

    public interface IReadOnlyHandler<T>
    {
        IObserver Subscribe(Action<T> action, List<IObserver> observers = null);
        void Unsubscribe(Action<T> action);
    }

    public class Handler<T> : IHandlerContainer<T> where T : struct
    {
        private readonly HashSet<Action<T>> _actions = new HashSet<Action<T>>();
        private readonly List<Action<T>> _forRemoving = new List<Action<T>>(5);
        private readonly List<Action<T>> _forAdding = new List<Action<T>>(5);

        private bool _isIterating;

        public static IObserver operator +(Handler<T> a, Action<T> b)
        {
            return a.Subscribe(b);
        }

        public static Handler<T> operator -(Handler<T> a, Action<T> b)
        {
            a.Unsubscribe(b);
            return a;
        }

        public IObserver Subscribe(Action<T> action, List<IObserver> observers = null)
        {
            var observer = _isIterating ? SafeSubscribe(action) : FullSubscribe(action);
            observers?.Add(observer);

            return observer;
        }

        public void Unsubscribe(Action<T> action)
        {
            if (_isIterating)
                SafeUnsubscribe(action);
            else
                FullUnsubscribe(action);
        }

        public void Clear()
        {
            RemoveCompletely();
            _forAdding.Clear();
            _actions.Clear();
        }

        public void Raise(in T argument)
        {
            _isIterating = true;
            foreach (var handler in _actions)
            {
                handler.Invoke(argument);
            }

            _isIterating = false;
            RemoveCompletely();
            AddCompletely();
        }

        private IObserver SafeSubscribe(Action<T> action)
        {
            if (_forRemoving.Contains(action)) 
                _forRemoving.Remove(action);

            if (!_actions.Contains(action) && !_forAdding.Contains(action))
                _forAdding.Add(action);
#if UNITY_EDITOR
            else
                Debug.LogWarningFormat("You are trying to re-subscribe a handler '{0}'.", action);
#endif
            return new Observer<T>(this, action);
        }

        private IObserver FullSubscribe(Action<T> action)
        {
            _actions.Add(action);
            return new Observer<T>(this, action);
        }

        private void SafeUnsubscribe(Action<T> action)
        {
            if (_actions.Contains(action))
                _forRemoving.Add(action);
#if UNITY_EDITOR
            else
                Debug.LogWarningFormat("You are trying to unsubscribe a handler '{0}' that is not subscribed.", action);
#endif
        }

        private void FullUnsubscribe(Action<T> action)
        {
            _actions.Remove(action);
        }

        private void RemoveCompletely()
        {
            using var action = _forRemoving.GetEnumerator();
            while (action.MoveNext())
            {
                FullUnsubscribe(action.Current);
            }

            _forRemoving.Clear();
        }

        private void AddCompletely()
        {
            using var action = _forAdding.GetEnumerator();
            while (action.MoveNext())
            {
                FullSubscribe(action.Current);
            }

            _forAdding.Clear();
        }
    }
    
    public interface IObserver : IDisposable
    {
    }

    public class Observer<T> : IObserver where T : struct
    {
        private readonly Handler<T> _handler;
        private readonly Action<T> _action;

        public Observer(Handler<T> handler, Action<T> action)
        {
            _handler = handler;
            _action = action;
        }

        public void Dispose()
        {
            _handler.Unsubscribe(_action);
        }
    }

    public static class Extensions
    {
        public static void ClearObservers(this List<IObserver> observers)
        {
            for (int i = 0; i < observers.Count; i++)
            {
                observers[i].Dispose();
            }
            observers.Clear();
        }
    }
}