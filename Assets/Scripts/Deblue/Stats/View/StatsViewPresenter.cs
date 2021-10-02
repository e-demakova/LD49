using System.Collections.Generic;
using Deblue.ObservingSystem;
using Deblue.UI.Views;
using UnityEngine;

namespace Deblue.Stats.View
{
    public class StatsViewPresenter<TEnum> : ModelViewPresenter<LimitedStatsStorage<TEnum>> where TEnum : System.Enum
    {
        [SerializeField] private StatView<TEnum>[] _views;

        private readonly Dictionary<TEnum, StatView<TEnum>> _statsViews = new Dictionary<TEnum, StatView<TEnum>>(5);
        private readonly List<IObserver> _observers = new List<IObserver>(2);

        protected override void MyInit()
        {
            for (int i = 0; i < Model.StatsIds.Count; i++)
            {
                InitStatView(Model.StatsIds[i]);
            }
            Model.StatsIds.ValueAdded.Subscribe(context => InitStatView(context.Value), _observers);
            Model.StatsIds.ValueRemoved.Subscribe(DestroyStatView, _observers);
        }

        public override void Dispose()
        {
            _observers.ClearObservers();
        }

        private void InitStatView(TEnum id)
        {
            if (_statsViews.ContainsKey(id))
            {
                return;
            }
            for (int i = 0; i < _views.Length; i++)
            {
                if (_views[i].StatId.Equals(id))
                {
                    AddViewToDictionary(id, i);
                    break;
                }
            }
        }

        private void AddViewToDictionary(TEnum id, int i)
        {
            _statsViews.Add(id, _views[i]);
            var stat = Model.GetStatProperty(id);
            _views[i].Init(stat);
        }

        private void DestroyStatView(ValueRemoved<int, TEnum> context)
        {
            if (!_statsViews.TryGetValue(context.Value, out var view))
                return;
            
            view.Dispose();
            Destroy(view.gameObject);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
