using UnityEngine;
using Zenject;
using UnityMVVM.ViewModelCore;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewManager.ViewLayer;
using System.Collections.Generic;
using UnityMVVM.ViewModelCore.ViewModelsFactory;
using System;

namespace UnityMVVM.DI
{
    /// <summary>
    /// Subcontainer for mvvm structure. It also modify parent container and inject there MVVM core control objects.
    /// </summary>
    public static class DiContainerExtensions
    {
        /// <summary>
        /// Configure container for MVVM pattern usage.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="layersData">Data about presentation layers.</param>
        public static void UseAsMvvmContainer(this DiContainer container, (string layerId, Transform layerContainer)[] layersData)
        {
            var viewModelsContainer = new DiContainer();
            var viewsContainer = new DiContainer();
            var layers = new IViewLayer[layersData.Length];
            for (int i = 0; i < layersData.Length; i++)
            {
                layers[i] = new ViewLayerImpl(layersData[i].layerId, layersData[i].layerContainer);
            }
            var viewsContainerAdapter = new ViewsContainerAdapter(viewsContainer);

            container.Bind<IViewsModelsContainerAdapter>().FromInstance(new ViewModelsContainerAdapter(viewModelsContainer)).AsSingle();
            container.Bind<IViewsContainerAdapter>().FromInstance(viewsContainerAdapter).AsSingle();
            container.Bind<IEnumerable<IViewLayer>>().FromInstance(layers).AsSingle().WhenInjectedInto<ViewManagerImpl>();
            container.FastBind<IViewManager, ViewManagerImpl>();

            viewModelsContainer.Bind<IViewsContainerAdapter>().FromInstance(viewsContainerAdapter).AsSingle();
        }

        /// <summary>
        /// Installs <see cref="IViewModelFactory{TViewModel}"/> for specified View-ViewModel pair.
        /// </summary>
        /// <param name="container">MVVM container to configure.</param>
        /// <param name="viewName">View identificator for openning.</param>
        /// <param name="viewPrefab">View prefab. It will be instantiated on creation. It should contains <typeparamref name="TView"/> component inside.</param>
        /// <typeparam name="TView">The type of a view</typeparam>
        /// <typeparam name="TViewModel">The type of a view model.</typeparam>
        /// <typeparam name="TViewModelImpl">The type, that implements a view model.</typeparam>
        public static void InstallView<TView, TViewModel, TViewModelImpl>(this DiContainer container, string viewName, GameObject viewPrefab)
        where TView : ViewBehaviour<TViewModel>
        where TViewModel : class, IViewModel
        where TViewModelImpl : class, TViewModel
        {
            var viewModelsContainer = container.TryResolve<IViewsModelsContainerAdapter>();
            if (viewModelsContainer == null)
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                    $"Use {nameof(UseAsMvvmContainer)} method to configure container.");
            var typesBindingList = new List<Type>
            {
                typeof(IViewModelFactoryInternal<TViewModel>),
                typeof(IViewModelFactory<TViewModel>),
            };
            if (typeof(TViewModel) != typeof(IViewModel))
            {
                typesBindingList.Add(typeof(IViewModelFactory<IViewModel>));
            }
            viewModelsContainer.Container
                .Bind(typesBindingList)
                .WithId(viewName)
                .To<ViewModelFactory<TView, TViewModel, TViewModelImpl>>()
                .AsSingle()
                .WithArgumentsExplicit(new []
                {
                    new TypeValuePair(typeof(GameObject), viewPrefab)
                });
        }

        /// <summary>
        /// Provides an access for specified objet types for view layer.
        /// </summary>
        /// <typeparam name="T">The type of dependency to be resolved in view layer.</typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use <see cref="UseAsMvvmContainer"/> to configure container.
        /// </exception>
        public static void ProvideAccessForViewLayer<T>(this DiContainer container)
        {
            var viewsContainer = container.TryResolve<IViewsContainerAdapter>();
            if (viewsContainer == null)
                throw new InvalidOperationException("Provided container does not contain container for the view layer. " +
                    $"Use {nameof(UseAsMvvmContainer)} to configure container.");
            viewsContainer.Container.Bind<T>().FromMethod(_ => container.Resolve<T>());
        }

        /// <summary>
        /// Provides an access for specified objet types for view layer.
        /// </summary>
        /// <typeparam name="TModelAcceesInterface">How the dependency is bound in the model layer.</typeparam>
        /// <typeparam name="TCommmonAccessInterface">How the dependency should be bound in the view layer.</typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use <see cref="UseAsMvvmContainer"/> to configure container.
        /// </exception>
        public static void ProvideAccessForViewLayer<TModelAcceesInterface, TCommmonAccessInterface>(this DiContainer container)
        {
            var viewsContainer = container.TryResolve<IViewsContainerAdapter>();
            if (viewsContainer == null)
                throw new InvalidOperationException("Provided container does not contain container for the view layer. " +
                    $"Use {nameof(UseAsMvvmContainer)} to configure container.");
            viewsContainer.Container.Bind<TCommmonAccessInterface>()
                .FromMethod(_ =>
                {
                    if (container.Resolve<TModelAcceesInterface>() is TCommmonAccessInterface common)
                    {
                        return common;
                    }
                    throw new Exception($"Can not convert dependency for view layer.");
                });
        }

        /// <summary>
        /// Provides an access for specified objet types for view-model layer.
        /// </summary>
        /// <typeparam name="T">The type of dependency to be resolved in view-model layer.</typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use <see cref="UseAsMvvmContainer"/> to configure container.
        /// </exception>
        public static void ProvideAccessForViewModelLayer<T>(this DiContainer container)
        {
            var viewModelsContainer = container.TryResolve<IViewsModelsContainerAdapter>();
            if (viewModelsContainer == null)
                throw new InvalidOperationException($"Provided container does not contain container for the view layer. " +
                    $"Use {nameof(UseAsMvvmContainer)} to configure container.");
            viewModelsContainer.Container.Bind<T>().FromMethod(_ => container.Resolve<T>());
        }

        /// <summary>
        /// Provides an access for specified objet types for view-model layer.
        /// </summary>
        /// <typeparam name="TModelAcceesInterface">How the dependency is bound in the model layer.</typeparam>
        /// <typeparam name="TCommmonAccessInterface">How the dependency should be bound in the view-model layer.</typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use <see cref="UseAsMvvmContainer"/> to configure container.
        /// </exception>
        public static void ProvideAccessForViewModelLayer<TModelAcceesInterface, TCommmonAccessInterface>(this DiContainer container)
        {
            var viewModelsContainer = container.TryResolve<IViewsModelsContainerAdapter>();
            if (viewModelsContainer == null)
                throw new InvalidOperationException($"Provided container does not contain container for the view layer. " +
                    $"Use {nameof(UseAsMvvmContainer)} to configure container.");
            viewModelsContainer.Container.Bind<TCommmonAccessInterface>()
                .FromMethod(_ =>
                {
                    if (container.Resolve<TModelAcceesInterface>() is TCommmonAccessInterface common)
                    {
                        return common;
                    }
                    throw new Exception($"Can not convert dependency for view-model layer.");
                });
        }

        /// <summary>
        /// Binds a dependency for both model and view-model layers.
        /// </summary>
        /// <typeparam name="TModelAccessInterface">The interface to access the dependency in the model layer.</typeparam>
        /// <typeparam name="TCommonAccessInterface">The interface to access the dependency in view-model layer.</typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use <see cref="UseAsMvvmContainer"/> to configure container.
        /// </exception>
        public static void FastBind<TModelAccessInterface, TCommonAccessInterface, TImpl>(this DiContainer container)
        where TImpl : TCommonAccessInterface, TModelAccessInterface
        {
            var viewModelsContainer = container.TryResolve<IViewsModelsContainerAdapter>();
            if (viewModelsContainer == null)
                throw new InvalidOperationException($"Provided container does not contain container for the view layer. " +
                    $"Use {nameof(UseAsMvvmContainer)} to configure container.");
            container.Bind(typeof(TModelAccessInterface), typeof(TCommonAccessInterface))
                .To<TImpl>().AsSingle();
            viewModelsContainer.Container.Bind<TCommonAccessInterface>()
                .FromMethod(_ =>
                {
                    if (container.Resolve<TModelAccessInterface>() is TCommonAccessInterface common)
                    {
                        return common;
                    }
                    throw new Exception($"Can not convert dependency for view-model layer.");
                }).AsSingle();
        }

        /// <summary>
        /// Binds a dependency for both model and common access layers.
        /// </summary>
        /// <typeparam name="TCommonAccessInterface">The interface to access the model in view-model layer.</typeparam>
        /// <typeparam name="TModelImpl"></typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use <see cref="UseAsMvvmContainer"/> to configure container.
        /// </exception>
        public static void FastBind<TCommonAccessInterface, TModelImpl>(this DiContainer container)
        where TModelImpl : TCommonAccessInterface
        {
            var viewModelsContainer = container.TryResolve<IViewsModelsContainerAdapter>();
            if (viewModelsContainer == null)
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                    $"Use {nameof(UseAsMvvmContainer)} mithod to configure container.");
            container.Bind<TCommonAccessInterface>().To<TModelImpl>().AsSingle();
            viewModelsContainer.Container.Bind<TCommonAccessInterface>()
                .FromMethod(_ => container.Resolve<TCommonAccessInterface>()).AsSingle();
        }

        /// <summary>
        /// Returens a container for view-model layer.
        /// </summary>
        /// <param name="container">The MVVM container.</param>
        public static DiContainer GetViewModelsContainer(this DiContainer container)
        {
            var viewModelsContainer = container.TryResolve<IViewsModelsContainerAdapter>();
            if (viewModelsContainer == null)
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                    $"Use {nameof(UseAsMvvmContainer)} method to configure container.");
            return viewModelsContainer.Container;
        }

        /// <summary>
        /// Returens a container for view layer.
        /// </summary>
        /// <param name="container">The MVVM container.</param>
        public static DiContainer GetViewsContainer(this DiContainer container)
        {
            var viewsContainer = container.TryResolve<IViewsContainerAdapter>();
            if (viewsContainer == null)
                throw new InvalidOperationException("Provided container does not contain container for the view layer. " +
                    $"Use {nameof(UseAsMvvmContainer)} to configure container.");
            return viewsContainer.Container;
        }
    }
}