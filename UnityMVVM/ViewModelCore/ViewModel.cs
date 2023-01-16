using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewManager.ViewLayer;

namespace UnityMVVM.ViewModelCore
{
    public abstract class ViewModel : IViewModel
    {
        private readonly IViewManager _viewManager;
        private readonly IViewLayer _layer;

        [AllowNull]
        private readonly IViewModel _parent;

        private bool _destroyed;

        public IViewLayer Layer => _layer;

        public event Action OnDestroy;

        public ViewModel(IViewManager viewManager, IViewLayer layer, [AllowNull] IViewModel parent)
        {
            _layer = layer;
            _viewManager = viewManager;
            _parent = parent;
            if (_parent != null)
            {
                _parent.OnDestroy += Destroy;
            }
        }

        protected T CreateSubView<T>() where T : class, IViewModel
        {
            var viewModel = _viewManager.Create<T>(this);
            return viewModel;
        }

        protected void OpenView<T>(string viewLayerId) where T : class, IViewModel
        {
            _viewManager.Open<T>(viewLayerId);
        }

        public void Destroy()
        {
            if (_destroyed)
            {
                Debug.LogException(new InvalidOperationException("Trying destroy already destroyed view model."));
            }
            _destroyed = true;
            OnDestroyInternal();
            OnDestroy?.Invoke();
        }

        protected virtual void OnDestroyInternal()
        {
            _parent.OnDestroy -= Destroy;
        }
    }
}
