using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;
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

        /// <summary>
        /// Current view model.
        /// </summary>
        [CanBeNull, AllowNull]
        protected T ViewModel { get; private set; }


        void IViewInitializer<T>.SetViewModel(T viewModel)
        {
            SetViewModelInternal(viewModel);
        }

        private void SetViewModelInternal([CanBeNull, AllowNull] T viewModel)
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
            ViewModel.OnDestroy += OnViewModelDestroyed;
        }

        private void OnViewModelDestroyed()
        {
            SetViewModelInternal(null);
            Destroy(this);
        }

        /// <summary>
        /// Called after view mdoel cleared. Before changing also.
        /// </summary>
        protected virtual void OnViewModelClear()
        {
            ViewModel.OnDestroy -= OnViewModelDestroyed;
        }
    }
}