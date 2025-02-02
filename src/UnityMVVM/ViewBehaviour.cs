using System;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Promises;
using UnityEngine;
using UnityMVVM.Pool;
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
        private IViewPool? _viewPool;
        private bool _isPartOfPooledView;

        private Vector3 _initialScale;

        private void Awake()
        {
            _initialScale = transform.localScale;
        }

        /// <summary>
        /// Current view model.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected T? ViewModel { get; private set; }

        /// <inheritdoc/>
        public string GetViewId()
        {
            return gameObject.name;
        }

        void IViewInitializer.SetViewModel(IViewModel viewModel)
        {
            SetViewModelInternal((T)viewModel);
        }

        /// <inheritdoc />
        public void SetParent(Transform parent)
        {
            var t = transform;
            t.SetParent(parent);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = _initialScale;
        }

        /// <inheritdoc />
        void IViewInitializer.SetPartOfPoolableView(bool isPartOfPooledView)
        {
            _isPartOfPooledView = isPartOfPooledView;
        }

        void IViewInitializer.SetPool(IViewPool? viewPool)
        {
            if (viewPool == null)
                return;
            
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (!(this is IPoolableView))
            {
                Debug.LogWarning($"The view {this.GetType().Name} does not implement {nameof(IPoolableView)} but the pool for this view is set!");
            }

            _viewPool = viewPool;
        }

        private void SetViewModelInternal(T? viewModel)
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
            ViewModel!.CloseStarted += OnCloseStarted;
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

        private void OnCloseStarted(IViewModel _)
        {
            Close().OnSuccess(() => ViewModel?.Destroy());
        }

        private void OnViewModelDestroyed(IViewModel _)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (this is IPoolableView poolableView)
            {
                SetViewModelInternal(null);
                if (_viewPool != null)
                {
                    _viewPool.Push(poolableView);
                }
                else
                {
                    Debug.LogError($"The view pool for poolable view {this.GetType().Name} is not set.");
                }
            }
            else
            {
                SetViewModelInternal(null);
                if (this && !_isPartOfPooledView)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        
        /// <summary>
        /// Called on Unity native representation of this object is being destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            var vm = ViewModel;
            SetViewModelInternal(null);
            vm?.Destroy();
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
        /// Called just before the view model destroyed.
        /// </summary>
        protected virtual void OnViewModelClear()
        {
            OnViewModelClearedInternal?.Invoke();
            ViewModel!.Destroyed -= OnViewModelDestroyed;
            ViewModel.CloseStarted -= OnCloseStarted;
        }
    }
}