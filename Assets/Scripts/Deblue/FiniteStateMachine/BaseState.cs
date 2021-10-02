using UnityEngine;

namespace Deblue.FiniteStateMachine
{
    public enum StateStatus
    {
        Defoult,
        Enter,
        Active,
        Exit
    }

    public interface IState
    {
        void Init();

        void Execute();

        void DeInit();
    }

    public abstract class BaseState : IState
    {
        public bool IsLooping => ActiveTime < 0;
        public bool StateTimeEnd;

        protected StateStatus Status = StateStatus.Defoult;
        protected IStateMachine Owner;

        protected float ActiveTime = -1;
        protected float EnterTime = -1;
        protected float ExitTime = -1;
        protected float MarkTime;

        //Вызывается стейтмашиной
        public abstract void DeInit();//При вызове переходит в "Exit" стейт
        public abstract void Init();//При вызове переходит в "Enter" стейт
        public abstract void Execute();

        //Вызывается самим скриптом
        protected abstract void AfterEnter();//Осуществляет переход из статуса "Enter" в "Active".
        protected abstract void AfterExit(); //Осуществляет переход из статуса "Exit" в "Defoult".
        protected abstract void PlayActiveBehaviour();
        protected abstract void PlayExitTransition();
        protected abstract void PlayEnterTransition();
    }

    public abstract class GenericState<T> : BaseState
    {
        protected T Executor;

        public GenericState(T executor, IStateMachine owner)
        {
            Executor = executor;
            Owner = owner;
        }

        public override sealed void Init()
        {
            MarkTime = Time.realtimeSinceStartup;
            Status = StateStatus.Enter;
        }

        public override sealed void DeInit()
        {
            MarkTime = Time.realtimeSinceStartup;
            Status = StateStatus.Exit;
            StateTimeEnd = false;
        }

        public override sealed void Execute()
        {
            switch (Status)
            {
                case StateStatus.Enter:
                    PlayEnterTransition();
                    if (Time.realtimeSinceStartup >= MarkTime + EnterTime)
                    {
                        MarkTime = Time.realtimeSinceStartup;
                        Status = StateStatus.Active;
                        AfterEnter();
                    }
                    break;

                case StateStatus.Active:
                    if (!IsLooping && Time.realtimeSinceStartup >= MarkTime + ActiveTime)
                    {
                        StateTimeEnd = true;
                    }
                    else
                    {
                        PlayActiveBehaviour();
                    }
                    break;

                case StateStatus.Exit:
                    PlayExitTransition();
                    if (Time.realtimeSinceStartup >= MarkTime + ExitTime)
                    {
                        Owner.SwitchStateToNext();
                        Status = StateStatus.Defoult;
                        AfterExit();
                    }
                    break;
            }
        }
    }
}