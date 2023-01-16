using UnityMVVM.ViewModelsFactory;
using UnityEngine;
using Zenject;
using UnityMVVM.ViewModelCore;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewManager.ViewLayer;
using System.Collections.Generic;

namespace UnityMVVM.DI
{
    public class MvvmInstallerHelper
    {
        private DiContainer _viewsContainer;

        public MvvmInstallerHelper(DiContainer container, (string layerId, Transform layerContainer)[] layersData)
        {
            _viewsContainer = container.CreateSubContainer();
            var layers = new IViewLayer[layersData.Length];
            for (int i = 0; i < layersData.Length; i++)
            {
                layers[i] = new ViewLayerImpl(layersData[i].layerId, layersData[i].layerContainer);
            }
            container.Bind<IViewsContainer>().FromInstance(new ViewsContainer(_viewsContainer));
            container.Bind<IEnumerable<IViewLayer>>().FromInstance(layers);
            container.Bind<IViewManager>().To<ViewManagerImpl>().AsSingle();
        }

        /// <summary>
        /// Installs <see cref="IViewModelFactory{TViewModel}"/> for specified View-ViewModel pair.
        /// </summary>
        /// <param name="viewPrefab">View prefab. It will be instantiated on creation. It should contains <see cref="TView"/> component inside.</param>
        /// <param name="container">The container for created objects.</param>
        /// <typeparam name="TView">The type of a view</typeparam>
        /// <typeparam name="TViewModel">The type of a view model.</typeparam>
        /// <typeparam name="TViewModelImpl">The type, that implements a view model.</typeparam>
        protected void InstallFactoryFor<TView, TViewModel, TViewModelImpl>(GameObject viewPrefab)
        where TView : ViewBehaviour<TViewModel>
        where TViewModel : class, IViewModel
        where TViewModelImpl : class, TViewModel
        {
            _viewsContainer.Bind<IViewModelFactory<TViewModel>>().To<ViewModelFactory<TView, TViewModel, TViewModelImpl>>()
                .AsSingle()
                .WithArgumentsExplicit(new []
                {
                    new TypeValuePair(typeof(GameObject), viewPrefab)
                });
        }
    }
}