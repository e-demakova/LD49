using System;
using System.Collections.Generic;
using System.Linq;
using Deblue.Input;
using Deblue.ObservingSystem;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Deblue.InteractiveObjects
{
    public class InteractObjectsSorter : IDisposable
    {
        private readonly List<InteractItem> _objects = new List<InteractItem>(30);
        private readonly List<IObserver> _observers = new List<IObserver>(30);
        private IInteractItem _highlightedObj;

        private IItemTaker _taker;

        private InteractItem NearInteractItem
        {
            get
            {
                var obj = _objects.FirstOrDefault(t => t.Order == InteractExecutionOrder.First);

                if (!obj)
                    obj = _objects.FirstOrDefault(t => t.Order == InteractExecutionOrder.Second);

                if (!obj)
                    obj = _objects.FirstOrDefault(t => t.Order == InteractExecutionOrder.Third);

                return obj;
            }
        }

        [Inject]
        protected void Construct(InputReceiver inputReceiver, IItemTaker taker)
        {
            _taker = taker;
            _objects.AddRange(Object.FindObjectsOfType<InteractItem>());

            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].Updated.Subscribe(context => UpdateHighlight(), _observers);
            }

            inputReceiver.SubscribeOnInput<ButtonDown>(Interact, KeyCode.F, _observers);
        }

        public void Dispose()
        {
            _observers.ClearObservers();
        }

        private void UpdateHighlight()
        {
            var obj = NearInteractItem;

            if (!obj)
                StopHighlightObj();
            else if (obj.CanHighlight(_taker))
                HighlightObject(obj);
        }

        private void StopHighlightObj()
        {
            _highlightedObj?.StopHighlight();
            _highlightedObj = null;
        }

        private void HighlightObject(IInteractItem obj)
        {
            obj.Highlight();
            _highlightedObj = obj;
        }

        private void Interact(ButtonDown context)
        {
            _highlightedObj?.Interact(_taker);
        }
    }
}