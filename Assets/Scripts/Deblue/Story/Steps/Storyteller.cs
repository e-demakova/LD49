using System;
using System.Collections.Generic;
using Deblue.ObservingSystem;
using Deblue.Story.DialogSystem;
using UnityEngine;
using Zenject;

namespace Deblue.Story.Steps
{
    public class Storyteller : IFixedTickable, IDisposable
    {
        private StepSO _currentStep;
        private readonly DialogRequester _dialogRequester;
        private readonly List<IObserver> _observers = new List<IObserver>(1);

        public Storyteller(DialogRequester dialogRequester, StepSO step)
        {
            _dialogRequester = dialogRequester;
            _currentStep = step;

            _dialogRequester.UpdateDialogues(_currentStep.UniqDialogsOnStep);
            _currentStep.Init();
        }

        public void Dispose()
        {
            _observers.ClearObservers();
            _currentStep.DeInit();
        }

        public void FixedTick()
        {
            _currentStep.Execute();
            if (_currentStep.IsDone)
                OnStepChange();
        }

        private void OnStepChange()
        {
            _currentStep.DeInit();
            _currentStep = _currentStep.NextStep;
            _dialogRequester.UpdateDialogues(_currentStep.UniqDialogsOnStep);
            _currentStep.Init();
        }
    }
}