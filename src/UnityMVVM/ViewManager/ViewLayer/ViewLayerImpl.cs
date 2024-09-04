using System;
using System.Threading.Tasks;
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

        public async ValueTask Clear()
        {
            if (_currentViewModel.Value == null)
            {
                return;
            }
            await _currentViewModel.Value.Close();
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
