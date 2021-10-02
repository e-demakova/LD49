using System;
using Deblue.Story.DialogSystem;
using UnityEngine;

namespace Deblue.Story.Steps
{
    [CreateAssetMenu(fileName = "Step_New", menuName = "Story/Step")]
    public class StepSO : ScriptableObject
    {
        [System.Serializable]
        public struct Condition
        {
            public ChangeStepConditionSO StepCondition;
            public StepSO NextStep;
        }

        public Condition[] Conditions = Array.Empty<Condition>();
        public Dialogs[] UniqDialogsOnStep = Array.Empty<Dialogs>();

        public bool IsDone { get; private set; }
        public StepSO NextStep { get; private set; }

        public void Init()
        {
            for (int i = 0; i < Conditions.Length; i++)
            {
                Conditions[i].StepCondition.Init();
            }
        }

        public void DeInit()
        {
            IsDone = false;
            for (int j = 0; j < Conditions.Length; j++)
            {
                Conditions[j].StepCondition.DeInit();
            }
        }

        public void Execute()
        {
            for (int i = 0; i < Conditions.Length; i++)
            {
                Conditions[i].StepCondition.CheckCondition();

                if (!Conditions[i].StepCondition.IsDone)
                    continue;

                IsDone = true;
                NextStep = Conditions[i].NextStep;
                return;
            }
        }
    }
}