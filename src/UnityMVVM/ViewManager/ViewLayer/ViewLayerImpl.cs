using AsyncReactAwait.Promises;
using System;
using AsyncReactAwait.Bindable;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager.ViewLayer
{
    internal class ViewLayerImpl : IViewLayer
    {

        private readonly IMutable<IViewModel?> _currentViewModel = new Mutable<IViewModel?>();

        public string Id { get; }
        public Transform Container { get; }
        public IBindable<IViewModel?> CurrentView => _currentViewModel;

        public ViewLayerImpl(string id, Transform container)
        {
            Id = id;
            Container = container;
        }

        public IPromise Clear()
        {
            if (_currentViewModel.Value == null)
            {
                var promise = new ControllablePromise();
                promise.Success();
                return promise;
            }
            return _currentViewModel.Value.Close();
        }

        public void ClearInstantly()
        {
           _currentViewModel.Value?.Destroy();
        }

        public void Set(IViewModel viewModel)
        {
            if (_currentViewModel.Value != null)
            {
                throw new InvalidOperationException("It is not possible to set new view model for layer, that already has view ");
            }
            _currentViewModel.Value = viewModel;
            _currentViewModel.Value.Destroyed += OnViewModelDestroyed;
        }

        private void OnViewModelDestroyed(IViewModel _)
        {
            if (_currentViewModel.Value == null) return;
            _currentViewModel.Value.Destroyed -= OnViewModelDestroyed;
            _currentViewModel.Value = null;
        }
    }
}
