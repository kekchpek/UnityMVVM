using AsyncReactAwait.Promises;
using JetBrains.Annotations;
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager.ViewLayer
{
    internal class ViewLayerImpl : IViewLayer
    {

        [AllowNull, CanBeNull]
        private IViewModel _currentViewModel;

        public string Id { get; }

        public Transform Container { get; }

        public ViewLayerImpl(string id, Transform container)
        {
            Id = id;
            Container = container;
        }

        public IPromise Clear()
        {
            if (_currentViewModel == null)
            {
                var promise = new ControllablePromise();
                promise.Success();
                return promise;
            }
            return _currentViewModel.Close();
        }

        public void ClearInstantly()
        {
           _currentViewModel.Destroy();
        }

        public void Set(IViewModel viewModel)
        {
            if (_currentViewModel != null)
            {
                throw new InvalidOperationException("It is not possible to set new view model for layer, that already has view ");
            }
            _currentViewModel = viewModel;
            _currentViewModel.Destroyed += OnViewModelDestoryed;
        }

        private void OnViewModelDestoryed()
        {
            _currentViewModel.Destroyed -= OnViewModelDestoryed;
            _currentViewModel = null;
        }
    }
}
