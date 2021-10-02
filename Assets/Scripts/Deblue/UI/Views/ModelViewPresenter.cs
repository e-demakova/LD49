using System;
using Deblue.Models;
using UnityEngine;

namespace Deblue.UI.Views
{
    public interface IView
    {
        void Show();
        void Hide();
    }

    public interface IModelView : IView
    {
        Type ModelType { get; }

        void Init(IModel model);
    }

    public abstract class UIView : MonoBehaviour, IView
    {
        public abstract void Hide();
        public abstract void Show();
    }

    public abstract class ModelView : MonoBehaviour, IModelView, IDisposable
    {
        public abstract Type ModelType { get; }
        public abstract void Init(IModel model);
        public abstract void Show();
        public abstract void Hide();
        public abstract void Dispose();
    }

    public abstract class ModelViewPresenter<T> : ModelView, IModelView where T : IModel
    {
        protected T Model;
        public override Type ModelType => typeof(T);

        public override void Init(IModel model)
        {
            if (model.GetType() != ModelType)
            {
                throw new ArgumentException($"Unexpected mode type {model.GetType().FullName}. Expecting {typeof(T).FullName}.");
            }

            Model = (T) model;

            MyInit();
        }

        public abstract override void Hide();

        public abstract override void Show();

        protected abstract void MyInit();
    }
}