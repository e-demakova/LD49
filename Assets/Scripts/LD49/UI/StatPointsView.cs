using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Deblue.ObservingSystem;
using Deblue.Stats.View;
using LD49.Stats;
using UnityEngine;

namespace LD49.UI
{
    public class StatPointsView : StatView<HeroStatId>
    {
        [SerializeField] private StatPoint _prefab;
        
        private readonly List<StatPoint> _points = new List<StatPoint>(5);
        
        protected override void Init()
        {
            InstantiatePoints();
            FillPoints();
        }
        
        public override void UpdateView(LimitedPropertyChanged<float> context)
        {
            InstantiatePoints();
            FillPoints();
        }

        private void InstantiatePoints()
        {
            var limit = Stat.UpperLimit > _points.Capacity ? _points.Capacity : Stat.UpperLimit;
            while (limit > _points.Count)
            {
                InstantiatePoint();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private StatPoint InstantiatePoint()
        {
            var point = Instantiate(_prefab, transform);
            _points.Add(point);
            return point;
        }

        private void FillPoints()
        {
            var fillBorder = Stat.Value >= _points.Count ? _points.Count : (int) Stat.Value;
            for (int i = 0; i < fillBorder; i++)
            {
                _points[i].IsFill = true;
            }

            for (int i = fillBorder; i < _points.Count; i++)
            {
                _points[i].IsFill = false;
            }
        }
    }
}