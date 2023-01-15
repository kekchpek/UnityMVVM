using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM
{
    public class ViewBehaviour<T> : MonoBehaviour, IViewInitializer<T> where T : class, IViewModel
    {

        [AllowNull]
        protected T ViewModel { get; private set; }


        void IViewInitializer<T>.SetViewModel(T viewModel)
        {
            SetViewModelInternal(viewModel);
        }

        private void SetViewModelInternal([AllowNull] T viewModel)
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

        protected virtual void OnViewModelSet()
        {
            ViewModel.OnDestroy += OnViewModelDestroyed;
        }

        private void OnViewModelDestroyed()
        {
            SetViewModelInternal(null);
            Destroy(this);
        }

        protected virtual void OnViewModelClear()
        {
            ViewModel.OnDestroy -= OnViewModelDestroyed;
        }
    }
}