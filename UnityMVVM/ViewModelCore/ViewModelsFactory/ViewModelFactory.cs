using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityMVVM.DI;
using UnityMVVM.ViewManager.ViewLayer;
using Zenject;

namespace UnityMVVM.ViewModelCore.ViewModelsFactory
{

    /// <inheritdoc cref="IViewModelFactory{TViewModel}"/>
    internal class ViewModelFactory<TView, TViewModel, TViewModelImpl> : IViewModelFactoryInternal<TViewModel>
        where TView : IViewInitializer<TViewModel>
        where TViewModel : IViewModel
        where TViewModelImpl : class, TViewModel
    {
        private readonly IInstantiator _instantiator;
        private readonly IViewsContainerAdapter _viewsContainerAdapter;
        private readonly GameObject _viewPrefab;

        /// <inheritdoc cref="IViewModelFactoryInternal{TViewModel}.ViewModelCreated"/>
        public event Action<TViewModel> ViewModelCreated;

        /// <summary>
        /// Default constructor for view factory.
        /// </summary>
        /// <param name="instantiator">Instantiator to create views.</param>
        /// <param name="viewsContainerAdapter"></param>
        /// <param name="viewPrefab">Prefab of the view.</param>
        public ViewModelFactory(IInstantiator instantiator, IViewsContainerAdapter viewsContainerAdapter, GameObject viewPrefab)
        {
            _instantiator = instantiator;
            _viewsContainerAdapter = viewsContainerAdapter;
            _viewPrefab = viewPrefab;
        }

        /// <inheritdoc cref="IViewModelFactory{TViewModel}.Create(IViewLayer, IViewModel, IPayload)"/>
        public TViewModel Create(IViewLayer viewLayer,
            [CanBeNull, AllowNull] IViewModel parent, 
            [CanBeNull, AllowNull] IPayload payload = null)
        {
            var implicitParams = new List<object>();
            if (parent != null)
            {
                implicitParams.Add(parent);
            }
            if (payload != null)
            {
                implicitParams.Add(payload);
            }
            implicitParams.Add(viewLayer);
            var viewModel = _instantiator.Instantiate<TViewModelImpl>(implicitParams);
            var view = _viewsContainerAdapter.Container.InstantiatePrefabForComponent<TView>(_viewPrefab, viewLayer.Container);
            if (viewModel is IInitializable initializable)
            {
                initializable.Initialize();
            }
            view.SetViewModel(viewModel);
            ViewModelCreated?.Invoke(viewModel);
            return viewModel;
        }
    }
}