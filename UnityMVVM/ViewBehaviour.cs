using System;
using UnityAuxiliaryTools.Promises;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM
{

    /// <summary>
    /// Base class for views.
    /// </summary>
    /// <typeparam name="T">The view model type for this view.</typeparam>
    public class ViewBehaviour<T> : MonoBehaviour, IViewInitializer<T> where T : class, IViewModel
    {

        private bool _isViewModelDestoroyed;
        private T _viewModel;

        /// <summary>
        /// Current view model.
        /// </summary>
        protected T ViewModel
        {
            get
            {
                if (_isViewModelDestoroyed)
                    throw new System.Exception("View model was destroyed!");
                return _viewModel;
            }
            private set
            {
                _viewModel = value;
            }
        }


        void IViewInitializer<T>.SetViewModel(T viewModel)
        {
            SetViewModelInternal(viewModel);
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
            ViewModel.Destroyed += OnViewModelDestroyed;
            ViewModel.CloseStarted += OnCloseStarted;
        }

        /// <summary>
        /// Method that starts and handle close proccess.
        /// </summary>
        /// <returns>Promise, that indicates closing proccess.</returns>
        protected virtual IPromise Close()
        {
            var promise = new ControllablePromise();
            promise.Success();
            return promise;
        }

        private void OnCloseStarted()
        {
            Close().OnSuccess(() => ViewModel.Destroy());
        }

        private void OnViewModelDestroyed()
        {
            OnViewModelClear();
            _isViewModelDestoroyed = true;
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Called just befor the view model and the view destroyed.
        /// </summary>
        protected virtual void OnViewModelClear()
        {
            ViewModel.Destroyed -= OnViewModelDestroyed;
            ViewModel.Destroyed -= OnCloseStarted;
        }
    }
}