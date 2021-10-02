using System.Collections.Generic;
using Deblue.Input;
using Deblue.ObservingSystem;
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

        public InputSender(InputReceiver inputReceiver, HeroController hero)
        {
            InputReceiver = inputReceiver;
            _hero = hero;
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
            _hero.SubscribeMovementOnInput(action => SubscribeInputDirection(action, _heroControlObservers));
        }
    }
}