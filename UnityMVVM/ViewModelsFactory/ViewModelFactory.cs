using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityMVVM.ViewModelCore;
using Zenject;

namespace UnityMVVM.ViewModelsFactory
{
    public class ViewModelFactory<TView, TViewModel, TViewModelImpl> : IViewModelFactory<TViewModel>
        where TView : IViewInitializer<TViewModel>
        where TViewModel : IViewModel
        where TViewModelImpl : class, TViewModel
    {
        private readonly IInstantiator _instantiator;
        private readonly GameObject _viewPrefab;

        public ViewModelFactory(IInstantiator instantiator, GameObject viewPrefab)
        {
            _instantiator = instantiator;
            _viewPrefab = viewPrefab;
        }

        public TViewModel Create(Transform viewContainer, IViewModel parent)
        {
            var viewModel = _instantiator.Instantiate<TViewModelImpl>(new object[] { parent });
            var view = _instantiator.InstantiatePrefabForComponent<TView>(_viewPrefab, viewContainer);
            view.SetViewModel(viewModel);
            return viewModel;
        }
    }
}