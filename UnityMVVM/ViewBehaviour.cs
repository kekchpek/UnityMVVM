using System;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Promises;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM
{
    /// <summary>
    /// Base class for views.
    /// </summary>
    /// <typeparam name="T">The view model type for this view.</typeparam>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class ViewBehaviour<T> : MonoBehaviour, IViewInitializer, IViewBehaviour where T : class, IViewModel
    {

        private event Action? OnViewModelClearedInternal;
        private bool _isViewModelDestroyed;
        private T? _viewModel;

        /// <summary>
        /// Current view model.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected T? ViewModel
        {
            get
            {
                if (_isViewModelDestroyed)
                    throw new Exception("View model was destroyed!");
                return _viewModel;
            }
            private set => _viewModel = value;
        }

        void IViewInitializer.SetViewModel(IViewModel viewModel)
        {
            SetViewModelInternal((T)viewModel);
        }

        private void SetViewModelInternal(T viewModel)
        {
            if (ViewModel != null)
            {
                OnViewModelClear();
            }
            ViewModel = viewModel;
            if (ViewModel != null)
            {
                OnViewModelSet();
            }
        }


        /// <summary>
        /// Called after view model changed.
        /// </summary>
        protected virtual void OnViewModelSet()
        {
            ViewModel!.Destroyed += OnViewModelDestroyed;
            ViewModel.CloseStarted += OnCloseStarted;
        }

        /// <summary>
        /// Method that starts and handle close process.
        /// </summary>
        /// <returns>Promise, that indicates closing process.</returns>
        protected virtual IPromise Close()
        {
            var promise = new ControllablePromise();
            promise.Success();
            return promise;
        }

        private void OnCloseStarted()
        {
            Close().OnSuccess(() => ViewModel?.Destroy());
        }

        private void OnViewModelDestroyed()
        {
            OnViewModelClear();
            _isViewModelDestroyed = true;
            if (this)
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Binds the handler to a bindable object and automatically unbind it on view model cleared.
        /// </summary>
        /// <param name="bindable">The bindable to bind to.</param>
        /// <param name="handler">The handler to being bound.</param>
        /// <param name="callImmediately">Pass true to call handler for current value.</param>
        /// <typeparam name="TBind">The type of bindable value.</typeparam>
        protected void SmartBind<TBind>(IBindable<TBind> bindable, Action handler, bool callImmediately = true)
        {
            bindable.Bind(handler, callImmediately);
            OnViewModelClearedInternal += () => bindable.Unbind(handler);
        }

        /// <summary>
        /// Binds the handler to a bindable object and automatically unbind it on view model cleared.
        /// </summary>
        /// <param name="bindable">The bindable to bind to.</param>
        /// <param name="handler">The handler to being bound.</param>
        /// <param name="callImmediately">Pass true to call handler for current value.</param>
        /// <typeparam name="TBind">The type of bindable value.</typeparam>
        protected void SmartBind<TBind>(IBindable<TBind> bindable, Action<TBind> handler, bool callImmediately = true)
        {
            bindable.Bind(handler, callImmediately);
            OnViewModelClearedInternal += () => bindable.Unbind(handler);
        }

        /// <summary>
        /// Binds the handler to a bindable object and automatically unbind it on view model cleared.
        /// </summary>
        /// <param name="bindable">The bindable to bind to.</param>
        /// <param name="handler">The handler to being bound.</param>
        /// <typeparam name="TBind">The type of bindable value.</typeparam>
        protected void SmartBind<TBind>(IBindable<TBind> bindable, Action<TBind, TBind> handler)
        {
            bindable.Bind(handler);
            OnViewModelClearedInternal += () => bindable.Unbind(handler);
        }

        /// <summary>
        /// Called just before the view model and the view destroyed.
        /// </summary>
        protected virtual void OnViewModelClear()
        {
            OnViewModelClearedInternal?.Invoke();
            ViewModel!.Destroyed -= OnViewModelDestroyed;
            ViewModel.CloseStarted -= OnCloseStarted;
        }
    }
}