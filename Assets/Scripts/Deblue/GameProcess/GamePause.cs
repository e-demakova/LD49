using System;
using System.Collections.Generic;
using Deblue.Input;
using UnityEngine;

using Deblue.ObservingSystem;

namespace Deblue.GameProcess
{
    public class GamePause
    {
        public bool IsPause => _isPause;
        private bool _isPause;

        private readonly Handler<GamePauseChange> _pauseChanged = new Handler<GamePauseChange>();
        public IReadOnlyHandler<GamePauseChange> PauseChanged => _pauseChanged;

        private void OnDestroy()
        {
            _pauseChanged.Clear();
        }

        private void SubscribePauseOnInput(Action<Action> subscription)
        {
            subscription.Invoke(ChangePause);
        }
        
        private void ChangePause()
        {
            _isPause = !_isPause;
            _pauseChanged.Raise(new GamePauseChange(_isPause));
        }
    }
}
