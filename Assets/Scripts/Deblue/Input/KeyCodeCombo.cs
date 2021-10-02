using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Deblue.Input
{
    public class KeyCodeCombo
    {
        private const float DefaultWaitingTime = 0.1f;

        public readonly Action ComboAction;

        public float WaitingTime = DefaultWaitingTime;

        private readonly KeyCode _codeA;
        private readonly KeyCode _codeB;

        private bool _codeAPressed;
        private bool _codeBPressed;

        public bool ComboDone => _codeAPressed && _codeBPressed && !WaitingTimeOver;

        public bool WaitingTimeOver => WaitingTime <= 0f;

        public bool IsFirstCodePressed => _codeAPressed ^ _codeBPressed;

        public KeyCodeCombo(KeyCode codeA, KeyCode codeB, Action comboAction)
        {
            _codeA = codeA;
            _codeB = codeB;
            ComboAction = comboAction;
        }

        public void SetCodeFlag(KeyCode code)
        {
            if (code == _codeA)
            {
                _codeAPressed = true;
            }
            else if (code == _codeB)
            {
                _codeBPressed = true;
            }
            else
            {
                throw new ArgumentException($"You are trying to check is cobo done by pushing {code}, but this KeyCod not valid. " +
                                            $"This combo consists of {_codeA} and {_codeB}.");
            }
        }

        public void ResetCombo()
        {
            _codeAPressed = false;
            _codeBPressed = false;
            WaitingTime = DefaultWaitingTime;
        }
    }

    public class VerticalAxisCombo
    {
        private Action[] _actions = new Action[3];

        public void SetAction(int direction, Action action)
        {
            _actions[GetIndex(direction)] = action;
        }

        public void Execute<T>(T context, Vector2Int direction) where T : struct
        {
            _actions[GetIndex(direction.y)]?.Invoke();
        }

        private int GetIndex(int direction)
        {
            if (direction == -1)
                return 0;
            if (direction == 0)
                return 1;
            if (direction == 1)
                return 2;

            throw new ArgumentException("Direction should be -1, 0 or 1, but " + direction + ".");
        }
    }
}