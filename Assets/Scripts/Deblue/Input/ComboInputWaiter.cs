using System;
using UnityEngine;

namespace Deblue.Input
{
    public class ComboInputWaiter
    {
        public Action CodeAction;
        public KeyCodeCombo Combo;

        public void Execute(float deltaTime)
        {
            if (Combo == null)
            {
                InvokeCodePressedAction();
                return;
            }

            if (Combo.IsFirstCodePressed)
                UpdateComboState(deltaTime);

            else if (Combo.ComboDone)
                InvokeComboActions();
        }

        private void UpdateComboState(float deltaTime)
        {
            Combo.WaitingTime -= deltaTime;

            if (Combo.WaitingTimeOver)
                InvokeCodePressedAction();
        }

        private void InvokeComboActions()
        {
            Combo.ComboAction.Invoke();
            Combo.ResetCombo();
        }

        private void InvokeCodePressedAction()
        {
            CodeAction?.Invoke();
            CodeAction = null;
            Combo?.ResetCombo();
        }
    }

    public class KeyCodeHolding
    {
        public struct Actions
        {
            public Action StartAction;
            public Action SucсessAction;
            public Action EndHoldingAction;
        }

        private readonly Actions _actions;
        private readonly float _timeToInvoke;

        private float _startHoldingTime;
        private float _endHoldingTime;

        public KeyCodeHolding(Actions actions, float timeToInvoke)
        {
            _timeToInvoke = timeToInvoke;
            _actions = actions;
        }

        private bool HoldButtonEnoughTime => _endHoldingTime - _startHoldingTime >= _timeToInvoke;

        public void StartTimer(ButtonDown context)
        {
            _startHoldingTime = Time.realtimeSinceStartup;
            _actions.StartAction?.Invoke();
        }

        public void Execute(OnButton context)
        {
            _endHoldingTime = Time.realtimeSinceStartup;
            
            if (HoldButtonEnoughTime)
                _actions.SucсessAction?.Invoke();
        }

        public void EndTimer(ButtonUp context)
        {
            _actions.EndHoldingAction?.Invoke();
        }
    }
}