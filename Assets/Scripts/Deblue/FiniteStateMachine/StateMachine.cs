namespace Deblue.FiniteStateMachine
{
    public interface IStateMachine
    {
        public BaseState CurrentState { get; }
        void Execute();
        void SetNextState(BaseState state);
        public void SwitchStateToNext();
    }

    public class StateMachine : IStateMachine
    {
        public BaseState CurrentState => _currentState;

        private BaseState _currentState;
        private BaseState _nextState;

        public void Execute()
        {
            _currentState.Execute();
        }

        public void SetNextState(BaseState state)
        {
            if (_currentState != state)
            {
                _nextState = state;
                _currentState?.DeInit();
            }
        }

        public void SwitchStateToNext()
        {
            _currentState = _nextState;
            _currentState?.Init();
            _nextState = null;
        }
    }
}