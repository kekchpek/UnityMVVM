using AsyncReactAwait.Promises;
using System;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager.ViewLayer
{
    internal class ViewLayerImpl : IViewLayer
    {

        private IViewModel? _currentViewModel;

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
           _currentViewModel?.Destroy();
        }

        public IViewModel? GetCurrentView()
        {
            return _currentViewModel;
        }

        public void Set(IViewModel viewModel)
        {
            if (_currentViewModel != null)
            {
                throw new InvalidOperationException("It is not possible to set new view model for layer, that already has view ");
            }
            _currentViewModel = viewModel;
            _currentViewModel.Destroyed += OnViewModelDestroyed;
        }

        private void OnViewModelDestroyed()
        {
            if (_currentViewModel == null) return;
            _currentViewModel.Destroyed -= OnViewModelDestroyed;
            _currentViewModel = null;
        }
    }
}
