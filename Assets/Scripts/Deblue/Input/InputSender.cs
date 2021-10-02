using System;
using System.Collections.Generic;
using Deblue.ObservingSystem;
using UnityEngine;
using Zenject;

namespace Deblue.Input
{
    public abstract class InputSender : IFixedTickable, IDisposable, IInitializable
    {
        private readonly Dictionary<KeyCode, ComboInputWaiter> _comboWaiters = new Dictionary<KeyCode, ComboInputWaiter>();
        private readonly Dictionary<KeyCode, VerticalAxisCombo> _verticalAxisCombos = new Dictionary<KeyCode, VerticalAxisCombo>();
        private readonly Dictionary<KeyCode, KeyCodeHolding> _holdingWaiters = new Dictionary<KeyCode, KeyCodeHolding>();
        protected InputReceiver InputReceiver;

        public void FixedTick()
        {
            foreach (var waiter in _comboWaiters)
            {
                waiter.Value.Execute(Time.fixedTime);
            }
        }

        public abstract void Initialize();
        public abstract void Dispose();

        protected abstract void DisableAllInput();


        protected void SubscribeOnButtonDown(Action action, KeyCode code, List<IObserver> observers)
        {
            InputReceiver.SubscribeOnInput<ButtonDown>(x => action.Invoke(), code, observers);
        }

        protected void SubscribeOnButtonDown(Action<ButtonDown> action, KeyCode code, List<IObserver> observers)
        {
            InputReceiver.SubscribeOnInput(action, code, observers);
        }

        protected void SubscribeOnButton(Action<OnButton> action, KeyCode code, List<IObserver> observers)
        {
            InputReceiver.SubscribeOnInput(action, code, observers);
        }

        protected void SubscribeOnButton(Action action, KeyCode code, List<IObserver> observers)
        {
            InputReceiver.SubscribeOnInput<OnButton>(x => action.Invoke(), code, observers);
        }

        protected void SubscribeOnButtonUp(Action action, KeyCode code, List<IObserver> observers)
        {
            InputReceiver.SubscribeOnInput<ButtonUp>(x => action.Invoke(), code, observers);
        }

        protected void SubscribeOnButtonUp(Action<ButtonUp> action, KeyCode code, List<IObserver> observers)
        {
            InputReceiver.SubscribeOnInput(action, code, observers);
        }

        protected void SubscribeInputDirection(Action<Vector2Int> action, List<IObserver> observers)
        {
            InputReceiver.SubscribeOnInputAxis(inputAxis => action.Invoke(inputAxis.Axis), observers);
        }

        protected void SubscribeOnCombo(Action action, KeyCode codeA, KeyCode codeB, List<IObserver> observers)
        {
            bool codeAWaiterExist = _comboWaiters.TryGetValue(codeA, out var waiter);
            bool codeBWaiterExist = _comboWaiters.TryGetValue(codeB, out waiter);

            if (!codeAWaiterExist && !codeBWaiterExist)
                waiter = AddWaiter(codeA);

            var combo = new KeyCodeCombo(codeA, codeB, action);
            waiter.Combo = combo;

            _comboWaiters.Add(codeBWaiterExist ? codeA : codeB, waiter);

            SubscribeOnButtonDown(x => combo.SetCodeFlag(codeA), codeA, observers);
            SubscribeOnButtonDown(x => combo.SetCodeFlag(codeB), codeB, observers);
        }

        protected void SubscribeInputOnWaitingCombo<T>(Action action, KeyCode code, List<IObserver> observers) where T : struct
        {
            if (!_comboWaiters.TryGetValue(code, out var waiter))
                waiter = AddWaiter(code);

            InputReceiver.SubscribeOnInput<T>(x => waiter.CodeAction = action, code, observers);
        }

        protected void SubscribeOnVerticalAxisCombo<T>(Action action, KeyCode code, int direction, List<IObserver> observers) where T : struct
        {
            if (!_verticalAxisCombos.TryGetValue(code, out var combo))
            {
                combo = new VerticalAxisCombo();
                _verticalAxisCombos.Add(code, combo);
            }

            InputReceiver.SubscribeOnInput<T>(x => combo.Execute(x, InputReceiver.InputDirection), code, observers);
            combo.SetAction(direction, action);
        }

        protected ComboInputWaiter AddWaiter(KeyCode code)
        {
            var waiter = new ComboInputWaiter();
            _comboWaiters.Add(code, waiter);
            return waiter;
        }

        protected void SubscribeOnHoldingButton(KeyCodeHolding.Actions actions, KeyCode code, float timeToInvoke, List<IObserver> observers)
        {
            if (!_holdingWaiters.TryGetValue(code, out var waiter))
            {
                waiter = new KeyCodeHolding(actions, timeToInvoke);
                _holdingWaiters.Add(code, waiter);
            }

            SubscribeOnButtonDown(waiter.StartTimer, code, observers);
            SubscribeOnButton(waiter.Execute, code, observers);
            SubscribeOnButtonUp(waiter.EndTimer, code, observers);
        }
    }
}