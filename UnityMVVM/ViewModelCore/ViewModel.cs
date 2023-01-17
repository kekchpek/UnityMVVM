using JetBrains.Annotations;
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewManager.ViewLayer;
using Zenject;

namespace UnityMVVM.ViewModelCore
{

    /// <summary>
    /// Base class for view model.
    /// </summary>
    public abstract class ViewModel : IViewModel
    {
        private IViewManager _viewManager;
        private IViewLayer _layer;

        [CanBeNull]
        private IViewModel _parent;

        private bool _destroyed;

        /// <inheritdoc cref="IViewModel.Layer"/>
        public IViewLayer Layer => _layer;

        /// <inheritdoc cref="IViewModel.OnDestroy"/>
        public event Action OnDestroy;


        /// <summary>
        /// Default constructor for view model.
        /// </summary>
        /// <param name="viewManager">View manager.</param>
        /// <param name="layer">Layer, on which it is placed.</param>
        /// <param name="parent">Parent view model. This view model will be destroyed with it.</param>
        [Inject]
        public void SetInternalDependencies(IViewManager viewManager, IViewLayer layer, [CanBeNull] IViewModel parent)
        {
            _layer = layer;
            _viewManager = viewManager;
            _parent = parent;
            if (_parent != null)
            {
                _parent.OnDestroy += Destroy;
            }
        }

        /// <summary>
        /// Creates a child view and view model.
        /// </summary>
        /// <typeparam name="T">Type of the view model.</typeparam>
        /// <returns>Created view model.</returns>
        protected T CreateSubView<T>() where T : class, IViewModel
        {
            var viewModel = _viewManager.Create<T>(this);
            return viewModel;
        }

        /// <summary>
        /// Clear layer and opens view on it.
        /// </summary>
        /// <typeparam name="T">Type of the view model.</typeparam>
        /// <param name="viewLayerId">Id of the layer to open view on.</param>
        protected void OpenView<T>(string viewLayerId) where T : class, IViewModel
        {
            _viewManager.Open<T>(viewLayerId);
        }

        /// <inheritdoc cref="IViewModel.Destroy"/>
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

        /// <summary>
        /// Internal method to handle view model destroying.
        /// </summary>
        protected virtual void OnDestroyInternal()
        {
            _parent.OnDestroy -= Destroy;
        }
    }
}
