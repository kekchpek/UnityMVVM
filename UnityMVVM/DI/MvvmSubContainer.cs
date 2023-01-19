using UnityEngine;
using Zenject;
using UnityMVVM.ViewModelCore;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewManager.ViewLayer;
using System.Collections.Generic;
using UnityMVVM.ViewModelCore.ViewModelsFactory;

namespace UnityMVVM.DI
{
    /// <summary>
    /// Subcontainer for mvvm structure. It also modify parent container and inject there MVVM core control objects.
    /// </summary>
    public class MvvmSubContainer
    {
        private DiContainer _viewsContainer;

        /// <summary>
        /// Defautl constroctor.
        /// </summary>
        /// <param name="container">The parent container. There are MVVM core control object will be injected there.</param>
        /// <param name="layersData">Data about presentation layers.</param>
        public MvvmSubContainer(DiContainer container, (string layerId, Transform layerContainer)[] layersData)
        {
            _viewsContainer = container.CreateSubContainer();
            var layers = new IViewLayer[layersData.Length];
            for (int i = 0; i < layersData.Length; i++)
            {
                layers[i] = new ViewLayerImpl(layersData[i].layerId, layersData[i].layerContainer);
            }
            container.Bind<IViewsContainerAdapter>().FromInstance(new ViewsContainerAdapter(_viewsContainer)).AsSingle();
            container.Bind<IEnumerable<IViewLayer>>().FromInstance(layers).AsSingle();
            container.Bind<IViewManager>().To<ViewManagerImpl>().AsSingle();
        }

        /// <summary>
        /// Installs <see cref="IViewModelFactory{TViewModel}"/> for specified View-ViewModel pair.
        /// </summary>
        /// <param name="viewPrefab">View prefab. It will be instantiated on creation. It should contains <typeparamref name="TView"/> component inside.</param>
        /// <typeparam name="TView">The type of a view</typeparam>
        /// <typeparam name="TViewModel">The type of a view model.</typeparam>
        /// <typeparam name="TViewModelImpl">The type, that implements a view model.</typeparam>
        public void InstallFactoryFor<TView, TViewModel, TViewModelImpl>(GameObject viewPrefab)
        where TView : ViewBehaviour<TViewModel>
        where TViewModel : class, IViewModel
        where TViewModelImpl : class, TViewModel
        {
            _viewsContainer.Bind(typeof(IViewModelFactoryInternal<TViewModel>), 
                typeof(IViewModelFactory<TViewModel>)).To<ViewModelFactory<TView, TViewModel, TViewModelImpl>>()
                .AsSingle()
                .WithArgumentsExplicit(new []
                {
                    new TypeValuePair(typeof(GameObject), viewPrefab)
                });
        }
    }
}