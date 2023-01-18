using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityMVVM.ViewModelCore;
using Zenject;

namespace UnityMVVM.ViewModelCore.ViewModelsFactory
{

    /// <inheritdoc cref="IViewModelFactory{TViewModel}"/>
    public class ViewModelFactory<TView, TViewModel, TViewModelImpl> : IViewModelFactoryInternal<TViewModel>
        where TView : IViewInitializer<TViewModel>
        where TViewModel : IViewModel
        where TViewModelImpl : class, TViewModel
    {
        private readonly IInstantiator _instantiator;
        private readonly GameObject _viewPrefab;

        /// <inheritdoc cref="IViewModelFactoryInternal{TViewModel}.ViewModelCreated"/>
        public event Action<TViewModel> ViewModelCreated;

        /// <summary>
        /// Default constructor for view factory.
        /// </summary>
        /// <param name="instantiator">Instantiator to create views.</param>
        /// <param name="viewPrefab">Prefab of the view.</param>
        public ViewModelFactory(IInstantiator instantiator, GameObject viewPrefab)
        {
            _instantiator = instantiator;
            _viewPrefab = viewPrefab;
        }

        /// <inheritdoc cref="IViewModelFactory{TViewModel}.Create(Transform, IViewModel, IPayload)"/>
        public TViewModel Create(Transform viewContainer,
            [CanBeNull, AllowNull] IViewModel parent, 
            [CanBeNull, AllowNull] IPayload payload = null)
        {
            var implicitParams = new List<object>();
            implicitParams.Add(parent);
            if (payload != null)
            {
                implicitParams.Add(payload);
            }
            var viewModel = _instantiator.Instantiate<TViewModelImpl>(implicitParams);
            var view = _instantiator.InstantiatePrefabForComponent<TView>(_viewPrefab, viewContainer);
            view.SetViewModel(viewModel);
            ViewModelCreated?.Invoke(viewModel);
            return viewModel;
        }
    }
}