using System;
using Deblue.ObservingSystem;
using UnityEngine;

namespace Deblue.Stats.View
{
    public abstract class BaseStatView : MonoBehaviour, IDisposable
    {
        public abstract void Init(IReadonlyObservLimitProperty<float> statProperty);
        public abstract void Dispose();
        public abstract void UpdateView(LimitedPropertyChanged<float> context);
    }

    public abstract class StatView<TEnum> : BaseStatView where TEnum : System.Enum
    {
        [SerializeField] private TEnum _statId;
        public TEnum StatId => _statId;

        protected IReadonlyObservLimitProperty<float> Stat;

        public sealed override void Init(IReadonlyObservLimitProperty<float> statProperty)
        {
            Stat = statProperty;
            Stat.PropertyChanged.Subscribe(UpdateView);
            Init();
        }

        protected abstract void Init();

        public sealed override void Dispose()
        {
            Stat.PropertyChanged.Unsubscribe(UpdateView);
        }
    }
}