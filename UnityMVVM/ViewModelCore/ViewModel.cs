using AsyncReactAwait.Promises;
using System;
using UnityEngine;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewManager.ViewLayer;
using Zenject;

namespace UnityMVVM.ViewModelCore
{

    /// <summary>
    /// Base class for view model.
    /// </summary>
    public class ViewModel : IViewModelInternal
    {
        private IViewManager _viewManager;
        private IViewLayer _layer;

        private IViewModel? _parent;

        private bool _destroyed;

        private IControllablePromise? _closePromise;

        /// <inheritdoc cref="IViewModel.Layer"/>
        public IViewLayer Layer => _layer;

        /// <inheritdoc cref="IViewModel.Destroyed"/>
        public event Action? Destroyed;

        /// <inheritdoc cref="IViewModel.CloseStarted"/>
        public event Action? CloseStarted;


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
                _parent.Destroyed += Destroy;
            }
        }

        /// <inheritdoc cref="CreateSubView(string, IPayload)"/>
        /// <typeparam name="T">Type of the view model.</typeparam>
        protected T CreateSubView<T>(string viewName, IPayload? payload = null) where T : class, IViewModel
        {
            var viewModel = _viewManager.Create<T>(this, viewName, payload);
            return viewModel;
        }

        /// <summary>
        /// Creates a child view and view model.
        /// </summary>
        /// <param name="viewName">The view identifier to open.</param>
        /// <param name="payload">The view model payload.</param>
        /// <returns>Created view model.</returns>
        protected IViewModel CreateSubView(string viewName, IPayload? payload = null)
        {
            var viewModel = _viewManager.Create(this, viewName, payload);
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
                _parent.Destroyed -= Destroy;
            }
            Destroyed?.Invoke();
            _closePromise?.Success();
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
            CloseStarted?.Invoke();
            return _closePromise;

        }

        /// <summary>
        /// Internal method to handle view model destroying.
        /// </summary>
        protected virtual void OnDestroyInternal()
        {

        }

        /// <summary>
        /// Protected method to handle close call.
        /// </summary>
        protected virtual void OnCloseStartedInternal()
        {

        }
        
        /// <summary>
        /// Protected method to handle view opened.
        /// </summary>
        protected virtual void OnOpenedInternal()
        {

        }

        /// <summary>
        /// Invoked by MVVM core when view is opened.
        /// </summary>
        public void OnOpened()
        {
            OnOpenedInternal();
        }
    }
}
