using System;
using System.Collections.Generic;

namespace Deblue.Input
{
    public readonly struct InputCanBeDequeue
    {
    }

    public interface IInputQueue
    {
        bool InputCanBeEnqueued { get; set; }
        bool InputCanBeExecute { get; set; }

        void Enqueue(Action action);
        void Dequeue(InputCanBeDequeue context);
    }

    public class LateInputQueue : IInputQueue
    {
        private Action _action;

        public bool InputCanBeEnqueued { get; set; }
        public bool InputCanBeExecute { get; set; }

        public void Enqueue(Action action)
        {
            if (InputCanBeEnqueued)
                _action = action;

            if (InputCanBeExecute)
                ExecuteInput();
        }

        public void Dequeue(InputCanBeDequeue context)
        {
            ExecuteInput();
        }

        private void ExecuteInput()
        {
            _action?.Invoke();
            _action = null;
        }
    }

    public class ComboAttackInputQueue : IInputQueue
    {
        private Queue<Action> _actions = new Queue<Action>(3);
        private int _capacity;

        public ComboAttackInputQueue(int capacity)
        {
            _capacity = capacity;
        }

        public bool InputCanBeEnqueued { get; set; }
        public bool InputCanBeExecute { get; set; }

        public void Enqueue(Action action)
        {
            if (InputCanBeEnqueued && _actions.Count <= _capacity)
                _actions.Enqueue(action);

            if (InputCanBeExecute)
                ExecuteInput();
        }

        public void Dequeue(InputCanBeDequeue context)
        {
            ExecuteInput();
        }
        
        private void ExecuteInput()
        {
            var action = _actions.Dequeue();
            action?.Invoke();
        }
    }
}