using System;
using System.Collections.Generic;

namespace Deblue.FiniteStateMachine
{
    public interface IStatesTable<in T>
    {
        void InitStates(T executor);
        void TryChangeState();
    }

    public abstract class StatesTable<TEnum, T> : IStatesTable<T> where TEnum : Enum
    {
        protected readonly T Executor;
        protected readonly IStateMachine StateMachine;
        protected readonly Dictionary<TEnum, BaseState> States = new Dictionary<TEnum, BaseState>(5);

        protected StatesTable(IStateMachine stateMachine, T executor)
        {
            Executor = executor;
            StateMachine = stateMachine;
            InitStates(executor);
        }

        public abstract void TryChangeState();
        public abstract void InitStates(T executor);

        protected void SetNextState(TEnum id)
        {
            StateMachine.SetNextState(States[id]);
        }
    }
}