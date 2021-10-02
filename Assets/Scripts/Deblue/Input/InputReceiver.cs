using System;
using System.Collections.Generic;
using Deblue.Extensions;
using Deblue.ObservingSystem;
using Deblue.SceneManagement;
using UnityEngine;
using Zenject;

namespace Deblue.Input
{
    public sealed class InputReceiver : ITickable, IFixedTickable, IDisposable
    {
        public Vector2Int InputDirection;
        
        private const string XAxis = "Horizontal";
        private const string YAxis = "Vertical";

        private readonly Dictionary<KeyCode, InputHandler> _handlers = new Dictionary<KeyCode, InputHandler>(5);
        private readonly List<InputHandler> _inputHandlers = new List<InputHandler>(5);

        private readonly Handler<InputDirection> _axisHandlers = new Handler<InputDirection>();
        private readonly Handler<MousePosition> _mouseHandlers = new Handler<MousePosition>();

        private bool _inputDisabled;
        private readonly List<IObserver> _observers = new List<IObserver>(5);

        public InputReceiver(SceneLoader sceneLoader)
        {
            sceneLoader.SceneLoadingStarted.Subscribe(context => _inputDisabled = true, _observers);
            sceneLoader.SceneLoaded.Subscribe(context => _inputDisabled = false, _observers);
        }

        public void Tick()
        {
            if (_inputDisabled)
                return;

            for (int i = 0; i < _inputHandlers.Count; i++)
            {
                _inputHandlers[i].OnButtonDown();
                _inputHandlers[i].OnButton();
                _inputHandlers[i].OnButtonUp();
            }
        }

        public void FixedTick()
        {
            if (_inputDisabled)
                return;

            int x = UnityEngine.Input.GetAxis(XAxis).GetClearDirection();
            int y = UnityEngine.Input.GetAxis(YAxis).GetClearDirection();

            InputDirection = new Vector2Int(x, y);
            _axisHandlers.Raise(new InputDirection(InputDirection));

            if (MainCamera.Camera == null)
                return;

            var mousePosition = UnityEngine.Input.mousePosition;
            mousePosition.z = 10;
            mousePosition = MainCamera.Camera.ScreenToWorldPoint(mousePosition);
            _mouseHandlers.Raise(new MousePosition(mousePosition));
        }

        public void Dispose()
        {
            for (int i = 0; i < _inputHandlers.Count; i++) 
                _inputHandlers[i].UnsubscribeAll();

            _axisHandlers.Clear();
            _observers.ClearObservers();
        }

        public void SubscribeOnInputAxis(Action<InputDirection> action, List<IObserver> observers = null)
        {
            _axisHandlers.Subscribe(action, observers);
        }

        public void SubscribeOnMousePosition(Action<MousePosition> action, List<IObserver> observers = null)
        {
            _mouseHandlers.Subscribe(action, observers);
        }

        public void SubscribeOnInput<T>(Action<T> action, KeyCode keyCode, List<IObserver> observers = null) where T : struct
        {
            if (!_handlers.TryGetValue(keyCode, out var handler))
            {
                handler = new InputHandler(keyCode);
                _handlers.Add(keyCode, handler);
                _inputHandlers.Add(handler);
            }

            handler.Subscribe(action, observers);
        }

        public void UnsubscribeOnInputAxis(Action<InputDirection> action)
        {
            _axisHandlers.Unsubscribe(action);
        }

        public void UnsubscribeOnMousePosition(Action<MousePosition> action)
        {
            _mouseHandlers.Unsubscribe(action);
        }

        public void UnsubscribeOnInput<T>(Action<T> action, KeyCode keyCode) where T : struct
        {
            if (_handlers.TryGetValue(keyCode, out var handler))
            {
                handler.Unsubscribe(action);
            }
        }
    }
}