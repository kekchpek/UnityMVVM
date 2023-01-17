using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityMVVM.ViewModelCore.ViewModelsFactory;
using Zenject;

namespace UnityMVVM.ViewModelCore.ViewModelsContainer
{
    internal class ViewModelsContainer<T> : IInitializable, IViewModelsContainer<T> where T : class, IViewModel
    {

        private readonly IViewModelFactoryInternal<T> _factory;

        private readonly HashSet<T> _viewModels = new HashSet<T>();

        public ViewModelsContainer(IViewModelFactoryInternal<T> viewModelFactory)
        {
            _factory = viewModelFactory;
        }

        public void Initialize()
        {
            _factory.ViewModelCreated += OnViewModelCreated;
        }

        private void OnViewModelCreated(T viewModel)
        {
            void OnViewModelDestroyed()
            {
                _viewModels.Remove(viewModel);
                viewModel.OnDestroy -= OnViewModelDestroyed;
            }
            viewModel.OnDestroy += OnViewModelDestroyed;
            _viewModels.Add(viewModel);
        }

        [CanBeNull]
        public T? Resolve()
        {
            if (_viewModels.Count== 0)
            {
                return null;
            }

            if (_viewModels.Count == 1)
            {
                return _viewModels.First();
            }

            throw new Exception("View model resolving conflict. Several view models presented.");
        }
    }
}
