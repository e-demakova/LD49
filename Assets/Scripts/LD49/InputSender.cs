using System;
using System.Collections.Generic;
using Deblue.Input;
using Deblue.ObservingSystem;
using Deblue.SceneManagement;
using LD49.Hero;
using UnityEngine;

namespace LD49
{
    public class InputSender : Deblue.Input.InputSender
    {
        private readonly HeroController _hero;

        private KeyCode _interactCode = KeyCode.F;
        private KeyCode _jumpCode = KeyCode.Space;

        private readonly List<IObserver> _heroControlObservers = new List<IObserver>(2);
        private readonly List<IObserver> _observers = new List<IObserver>(10);

        public InputSender(InputReceiver inputReceiver, HeroController hero, SceneLoader sceneLoader)
        {
            InputReceiver = inputReceiver;
            _hero = hero;

            sceneLoader.SceneLoadingStarted.Subscribe(x => DisableAllInput(), _observers);
            sceneLoader.SceneLoaded.Subscribe(x => SubscribeHeroControl(), _observers);
        }

        public override void Initialize()
        {
            SubscribeHeroControl();
        }

        public override void Dispose()
        {
            DisableAllInput();
            _observers.ClearObservers();
        }

        protected override void DisableAllInput()
        {
            _heroControlObservers.ClearObservers();
        }

        private void SubscribeHeroControl()
        {
            _hero.SubscribeJumpOnInput(action => SubscribeOnButtonDown(action, _jumpCode, _heroControlObservers));
            _hero.SubscribeInteractionOnInput(action => SubscribeOnButtonDown(action, _interactCode, _heroControlObservers));
            _hero.SubscribeMovementOnInput(action => SubscribeInputDirection(action, _heroControlObservers));
        }
    }
}