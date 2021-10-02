using System;
using System.Collections.Generic;
using Deblue.ObservingSystem;
using UnityEngine;

namespace Deblue.Input
{
    //Можно было бы наследоваться от EventSender, но тут все ивенты заранее предопределены
    //и вызываться будут скорее всего довольно часто, так что для избежания лишних проверок
    //было решено не связывать эти классы.
    public class InputHandler
    {
        public KeyCode KeyCode;

        private readonly Handler<ButtonDown> _onButtonDown = new Handler<ButtonDown>();
        private readonly Handler<OnButton> _onButton = new Handler<OnButton>();
        private readonly Handler<ButtonUp> _onButtonUp = new Handler<ButtonUp>();

        public InputHandler(KeyCode keyCode)
        {
            KeyCode = keyCode;
        }

        public void OnButtonDown()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode))
            {
                _onButtonDown.Raise(new ButtonDown(KeyCode));
            }
        }

        public void OnButton()
        {
            if (UnityEngine.Input.GetKey(KeyCode))
            {
                _onButton.Raise(new OnButton(KeyCode));
            }
        }

        public void OnButtonUp()
        {
            if (UnityEngine.Input.GetKeyUp(KeyCode))
            {
                _onButtonUp.Raise(new ButtonUp(KeyCode));
            }
        }

        #region Subscribing
        public void Subscribe<T>(Action<T> action, List<IObserver> observers = null) where T : struct
        {
            var actionOnDown = action as Action<ButtonDown>;
            if (actionOnDown != null)
            {
                SubscribeOnButtonDown(actionOnDown, observers);
                return;
            }

            var actionOnButton = action as Action<OnButton>;
            if (actionOnButton != null)
            {
                SubscribeOnButton(actionOnButton, observers);
                return;
            }

            var actionOnUp = action as Action<ButtonUp>;
            if (actionOnUp != null)
            {
                SubscribeOnButtonUp(actionOnUp, observers);
                return;
            }

            ThrowUnexpectedTypeException(typeof(T));
        }

        private void SubscribeOnButtonDown(Action<ButtonDown> action, List<IObserver> observers = null)
        {
            _onButtonDown.Subscribe(action, observers);
        }

        private void SubscribeOnButton(Action<OnButton> action, List<IObserver> observers = null)
        {
            _onButton.Subscribe(action, observers);
        }

        private void SubscribeOnButtonUp(Action<ButtonUp> action, List<IObserver> observers = null)
        {
            _onButtonUp.Subscribe(action, observers);
        }
        #endregion

        #region Unsubscribing
        public void UnsubscribeAll()
        {
            _onButtonDown.Clear();
            _onButton.Clear();
            _onButtonUp.Clear();
        }

        public void Unsubscribe<T>(Action<T> action) where T : struct
        {
            var actionOnDown = action as Action<ButtonDown>;
            if (actionOnDown != null)
            {
                UnsubscribeOnButtonDown(actionOnDown);
                return;
            }

            var actionOnButton = action as Action<OnButton>;
            if (actionOnButton != null)
            {
                UnsubscribeOnButton(actionOnButton);
                return;
            }

            var actionOnUp = action as Action<ButtonUp>;
            if (actionOnUp != null)
            {
                UnsubscribeOnButtonUp(actionOnUp);
                return;
            }

            ThrowUnexpectedTypeException(typeof(T));
        }

        private void UnsubscribeOnButtonDown(Action<ButtonDown> action)
        {
            _onButtonDown.Unsubscribe(action);
        }

        private void UnsubscribeOnButton(Action<OnButton> action)
        {
            _onButton.Unsubscribe(action);
        }

        private void UnsubscribeOnButtonUp(Action<ButtonUp> action)
        {
            _onButtonUp.Unsubscribe(action);
        }
        #endregion

        private void ThrowUnexpectedTypeException(Type type)
        {
            throw new ArgumentException($"Unexpected input type {type}.");
        }
    }
}
