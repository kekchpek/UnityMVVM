using AsyncReactAwait.Promises;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewManager.ViewLayer;
using Zenject;

// ReSharper disable MemberCanBePrivate.Global

namespace UnityMVVM.ViewModelCore
{

    /// <summary>
    /// Base class for view model.
    /// </summary>
    public class ViewModel : IViewModel
    {
        private IViewManager _viewManager = null!;
        private IViewLayer _layer = null!;

        private readonly HashSet<IViewModel> _subviews = UnityEngine.Pool.HashSetPool<IViewModel>.Get();

        private IViewModel? _parent;

        private bool _destroyed;

        private IControllablePromise? _closePromise;

        /// <inheritdoc cref="IViewModel.Layer"/>
        public IViewLayer Layer => _layer;

        /// <inheritdoc cref="IViewModel.Destroyed"/>
        public event Action<IViewModel>? Destroyed;

        /// <inheritdoc cref="IViewModel.CloseStarted"/>
        public event Action<IViewModel>? CloseStarted;


        /// <summary>
        /// Default constructor for view model.
        /// </summary>
        /// <param name="viewManager">View manager.</param>
        /// <param name="layer">Layer, on which it is placed.</param>
        /// <param name="parent">Parent view model. This view model will be destroyed with it.</param>
        [Inject]
        public void SetInternalDependencies(IViewManager viewManager, IViewLayer layer, 
            [InjectOptional] IViewModel? parent)
        {
            _layer = layer;
            _viewManager = viewManager;
            _parent = parent;
            if (_parent != null)
            {
                _parent.Destroyed += OnParentDestroyed;
            }
        }

        /// <summary>
        /// Invoked by MVVM core when view is opened.
        /// </summary>
        void IViewModel.OnOpened()
        {
            OnOpenedInternal();
        }

        /// <inheritdoc cref="CreateSubView(string, IPayload)"/>
        /// <typeparam name="T">Type of the view model.</typeparam>
        protected T CreateSubView<T>(string viewName, IPayload? payload = null) where T : class, IViewModel
        {
            return CreateSubView<T>(viewName, _layer.Container, payload);
        }

        /// <summary>
        /// Creates a child view and view model.
        /// </summary>
        /// <param name="viewName">The view identifier to open.</param>
        /// <param name="payload">The view model payload.</param>
        /// <returns>Created view model.</returns>
        protected IViewModel CreateSubView(string viewName, IPayload? payload = null)
        {
            return CreateSubView(viewName, _layer.Container, payload);
        }

        /// <inheritdoc cref="CreateSubView(string,UnityMVVM.ViewModelCore.IPayload?)"/>
        /// <param name="container">The container to instantiate view to.</param>
        protected IViewModel CreateSubView(
#pragma warning disable CS1573
            string viewName, 
#pragma warning restore CS1573
            Transform container, 
#pragma warning disable CS1573
            IPayload? payload = null)
#pragma warning restore CS1573
        {
            var viewModel = _viewManager.Create(this, viewName, container, payload);
            return viewModel;
        }
        
        /// <inheritdoc cref="CreateSubView{T}(string,UnityMVVM.ViewModelCore.IPayload?)"/>
        /// <param name="container">The container to instantiate view to.</param>
        protected T CreateSubView<T>(
#pragma warning disable CS1573
            string viewName, 
#pragma warning restore CS1573
            Transform container, 
#pragma warning disable CS1573
            IPayload? payload = null) 
#pragma warning restore CS1573
            where T : class, IViewModel
        {
            var viewModel = _viewManager.Create<T>(this, viewName, container, payload);
            return viewModel;
        }

        /// <summary>
        /// Clear layer and opens view on it.
        /// </summary>
        /// <param name="viewLayerId">Id of the layer to open view on.</param>
        /// <param name="viewName">The view identifier to open.</param>
        /// <param name="payload">The view model payload.</param>
        protected async IPromise OpenView(string viewLayerId, string viewName, IPayload? payload = null)
        {
            await _viewManager.Open(viewLayerId, viewName, payload);
        }

        /// <inheritdoc />
        public T? GetSubview<T>() where T : IViewModel
        {
            return (T?)_subviews.FirstOrDefault(x => x is T);
        }

        /// <inheritdoc />
        public T[] GetSubviews<T>() where T : IViewModel
        {
            return _subviews
                .Where(x => x is T)
                .Cast<T>()
                .ToArray();
        }

        /// <summary>
        /// Protected method to handle close call.
        /// </summary>
        protected virtual void OnCloseStartedInternal()
        {
            // Do noting.
            // Supposed to be overriden.
        }
        
        /// <summary>
        /// Protected method to handle view opened.
        /// </summary>
        protected virtual void OnOpenedInternal()
        {
            // Do noting.
            // Supposed to be overriden.
        }

        void IViewModel.AddSubview(IViewModel subview)
        {
            subview.Destroyed += OnSubviewDestroyed;
            _subviews.Add(subview);
        }

        private void OnSubviewDestroyed(IViewModel subview)
        {
            if (!_subviews.Contains(subview))
                Debug.LogError("Subview destruction handler is called for not a subview.");
            subview.Destroyed -= OnSubviewDestroyed;
            _subviews.Remove(subview);
        }

        /// <inheritdoc cref="IViewModel.Close"/>
        public IPromise Close()
        {
            if (_closePromise != null)
            {
                return _closePromise;
            }
            _closePromise = new ControllablePromise();
            OnCloseStartedInternal();
            CloseStarted?.Invoke(this);
            return _closePromise;

        }

        private void OnParentDestroyed(IViewModel _)
        {
            Destroy();
        }

        /// <summary>
        /// Internal method to handle view model destroying.
        /// </summary>
        protected virtual void OnDestroyInternal()
        {
            // Do noting.
            // Supposed to be overriden.
        }

        /// <inheritdoc cref="IViewModel.Destroy"/>
        public void Destroy()
        {
            if (_destroyed)
            {
                Debug.LogException(new InvalidOperationException("Trying destroy already destroyed view model."));
            }
            OnDestroyInternal();
            _destroyed = true;
            if (_parent != null)
            {
                _parent.Destroyed -= OnParentDestroyed;
            }
            foreach (var subview in _subviews)
            {
                subview.Destroyed -= OnSubviewDestroyed;
            }
            _subviews.Clear();
            Destroyed?.Invoke(this);
            _closePromise?.Success();
        }
    }
}
