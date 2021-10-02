using System;
using System.Collections.Generic;
using Deblue.Models;
using Deblue.UI.Views;
using UnityEngine;

namespace Deblue.UI
{
    public class UIVisualizator : MonoBehaviour
    {
        [SerializeField] private ModelView[] _viewsPresenters;
        private readonly Dictionary<Type, IView> _views = new Dictionary<Type, IView>();

        public void InitView<T>(T model) where T : IModel
        {
            for (int i = 0; i < _viewsPresenters.Length; i++)
            {
                if (_viewsPresenters[i].ModelType == typeof(T))
                {
                    _viewsPresenters[i].Init(model);
                    _views.Add(_viewsPresenters[i].ModelType, _viewsPresenters[i]);
                    return;
                }
            }
            throw new ArgumentException($"View with given type not found {typeof(T)}");
        }

        public void Show<T>() where T : IModel
        {
            if (_views.TryGetValue(typeof(T), out var view))
            {
                view.Show();
            }
            else
            {
                throw new ArgumentException($"No registered view for model type {typeof(T)}");
            }
        }

        public void Hide<T>() where T : IModel
        {
            if (_views.TryGetValue(typeof(T), out var view))
            {
                view.Hide();
            }
            else
            {
                throw new ArgumentException($"No registered view model type {typeof(T)}");
            }
        }
    }
}
