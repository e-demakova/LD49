using UnityEngine;

namespace Deblue.Story.Steps
{
    public abstract class ChangeStepConditionSO : ScriptableObject
    {
        public bool IsDone { get; protected set; }

        public void Init()
        {
            MyInit();
        }

        protected virtual void MyInit()
        {
        }

        public void DeInit()
        {
            IsDone = false;
            MyDeInit();
        }

        protected abstract void MyDeInit();

        public virtual void CheckCondition()
        {
            
        }
    }
}