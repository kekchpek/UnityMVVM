using UnityEngine;
using Zenject;
using UnityMVVM.ViewModelCore;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewManager.ViewLayer;
using System.Collections.Generic;
using UnityMVVM.ViewModelCore.ViewModelsFactory;
using System;
using System.Linq;
using ModestTree;
using UnityMVVM.DI.Config;
using UnityMVVM.DI.Environment;
using UnityMVVM.DI.Mapper;
using UnityMVVM.Pool;

// ReSharper disable MemberCanBePrivate.Global

namespace UnityMVVM.DI
{
    /// <summary>
    /// Subcontainer for mvvm structure. It also modify parent container and inject there MVVM core control objects.
    /// </summary>
    public static class DiContainerExtensions
    {

        private static readonly Dictionary<DiContainer, IContainerEnvironment> ContainerEnvironments = new();

        /// <summary>
        /// Configure container for MVVM pattern usage.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="layersData">Data about presentation layers.</param>
        /// <param name="config">Configuration for MVVM container.</param>
        public static void UseAsMvvmContainer(
            this DiContainer container,
            (string layerId, Transform layerContainer)[] layersData,
            MvvmContainerConfiguration config = default)
        {
            if (ContainerEnvironments.ContainsKey(container))
            {
                throw new Exception("The container is already used as MVVM container");
            }
            var layers = new IViewLayer[layersData.Length];
            for (int i = 0; i < layersData.Length; i++)
            {
                layers[i] = new ViewLayerImpl(layersData[i].layerId, layersData[i].layerContainer);
            }
            container.UseAsMvvmContainer(layers, config);
        }

        /// <summary>
        /// Configure container for MVVM pattern usage.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="layers">View layers.</param>
        /// <param name="config">Configuration for MVVM container.</param>
        public static void UseAsMvvmContainer(
            this DiContainer container,
            IEnumerable<IViewLayer> layers,
            MvvmContainerConfiguration config)
        {
            var viewModelsContainer = new DiContainer();
            var viewsContainer = new DiContainer();
            var viewsContainerAdapter = new ViewsContainerAdapter(viewsContainer);
            var viewModelsContainerAdapter = new ViewModelsContainerAdapter(viewModelsContainer);
            var mapper = new ViewToViewModelMapper();

            var env = new ContainerEnvironment(mapper, viewsContainerAdapter, viewModelsContainerAdapter);
            
            ContainerEnvironments.Add(container, env);
            
            container.Bind<IEnumerable<IViewLayer>>().FromInstance(layers).AsSingle().WhenInjectedInto<ViewManagerImpl>();
            container.Bind<IViewsModelsContainerAdapter>().FromInstance(viewModelsContainerAdapter);
            container.FastBind<IViewManager, ViewManagerImpl>();

            if (config.ViewFactory == null)
            {
                viewModelsContainer.Bind<IViewFactory>().To<ViewFactory>().AsSingle();
            }
            else
            {
                viewModelsContainer.Bind<IViewFactory>().FromInstance(config.ViewFactory);
            }
            
            viewModelsContainer.Bind<IViewToViewModelMapper>().FromInstance(mapper);
            viewModelsContainer.Bind<IViewsContainerAdapter>().FromInstance(viewsContainerAdapter).AsSingle();
        }

        /// <summary>
        /// Installs <see cref="IViewModelsFactory"/> for specified View-ViewModel pair.
        /// </summary>
        /// <param name="container">MVVM container to configure.</param>
        /// <param name="viewName">View identificator for opening.</param>
        /// <param name="viewPrefab">View prefab. It will be instantiated on creation. It should contains <typeparamref name="TView"/> component inside.</param>
        /// <typeparam name="TView">The type of a view</typeparam>
        /// <typeparam name="TViewModel">The type of a view model.</typeparam>
        /// <typeparam name="TViewModelImpl">The type, that implements a view model.</typeparam>
        public static void InstallView<TView, TViewModel, TViewModelImpl>(this DiContainer container, string viewName, GameObject viewPrefab)
        where TView : ViewBehaviour<TViewModel>
        where TViewModel : class, IViewModel
        where TViewModelImpl : class, TViewModel
        {
             container.InstallView<TView, TViewModel, TViewModelImpl>(viewName, () => viewPrefab);
        }

        /// <summary>
        /// Installs <see cref="IViewModelsFactory"/> for specified View-ViewModel pair.
        /// </summary>
        /// <param name="container">MVVM container to configure.</param>
        /// <param name="viewName">View identificator for opening.</param>
        /// <param name="viewPrefabGetter">The method to obtain view prefab. View should contains <typeparamref name="TView"/> component inside.</param>
        /// <typeparam name="TView">The type of a view</typeparam>
        /// <typeparam name="TViewModel">The type of a view model.</typeparam>
        /// <typeparam name="TViewModelImpl">The type, that implements a view model.</typeparam>
        public static void InstallView<TView, TViewModel, TViewModelImpl>(this DiContainer container, string viewName, Func<GameObject> viewPrefabGetter)
            where TView : ViewBehaviour<TViewModel>
            where TViewModel : class, IViewModel
            where TViewModelImpl : class, TViewModel
        {
            InstallViewInternal<TView, TViewModel, TViewModelImpl>(container, viewName, viewPrefabGetter, null);
        }

        /// <inheritdoc cref="InstallView{TView,TViewModel,TViewModelImpl}(Zenject.DiContainer,string,Func{UnityEngine.GameObject})"/>
        /// <param name="viewPool">The pool for views. Uses default <see cref="ViewPool{T}"/> object if null specified.</param>
        public static void InstallPoolableView
            <TView, TViewModel, TViewModelImpl>
#pragma warning disable CS1573
            (this DiContainer container,
            string viewName,
            Func<GameObject> viewPrefabGetter,
#pragma warning restore CS1573
            IViewPool? viewPool = null)
            where TView : ViewBehaviour<TViewModel>, IPoolableView
            where TViewModel : class, IViewModel
            where TViewModelImpl : class, TViewModel
        {
            InstallViewInternal<TView, TViewModel, TViewModelImpl>(
                container, 
                viewName,
                viewPrefabGetter,
                viewPool ?? new ViewPool<TView>());
        }
        
        /// <inheritdoc cref="InstallView{TView,TViewModel,TViewModelImpl}(Zenject.DiContainer,string,UnityEngine.GameObject)"/>
        /// <param name="viewPool">The pool for views. Uses default <see cref="ViewPool{T}"/> object if null specified.</param>
        public static void InstallPoolableView
            <TView, TViewModel, TViewModelImpl>
#pragma warning disable CS1573
            (this DiContainer container,
                string viewName,
                GameObject viewPrefab,
#pragma warning restore CS1573
                IViewPool? viewPool = null)
            where TView : ViewBehaviour<TViewModel>, IPoolableView
            where TViewModel : class, IViewModel
            where TViewModelImpl : class, TViewModel
        {
            InstallViewInternal<TView, TViewModel, TViewModelImpl>(
                container, 
                viewName,
                () => viewPrefab,
                viewPool ?? new ViewPool<TView>());
        }

        private static void InstallViewInternal<TView, TViewModel, TViewModelImpl>(DiContainer container, string viewName, Func<GameObject> viewPrefabGetter, IViewPool? viewPool)
            where TView : ViewBehaviour<TViewModel>
            where TViewModel : class, IViewModel
            where TViewModelImpl : class, TViewModel
        {
            if (!ContainerEnvironments.TryGetValue(container, out var env))
            {
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                                                    $"Use {nameof(UseAsMvvmContainer)} method to configure container.");
            }
            var viewModelsContainer = env.ViewsModelsContainerAdapter;
            viewModelsContainer.Container
                .Bind<IViewModelsFactory>()
                .WithId(viewName)
                .To<ViewModelsFactory<TView>>()
                .AsTransient()
                .WithArgumentsExplicit(new []
                {
                    new TypeValuePair(typeof(Func<GameObject>), viewPrefabGetter),
                    new TypeValuePair(typeof(IViewPool), viewPool),
                });
            env.Mapper.Map<TView, TViewModelImpl>();
        }

        /// <summary>
        /// Installs <see cref="IViewModelsFactory"/> for specified View-ViewModel pair.
        /// </summary>
        /// <param name="container">MVVM container to configure.</param>
        /// <typeparam name="TView">The type of a view</typeparam>
        /// <typeparam name="TViewModel">The type of a view model.</typeparam>
        /// <typeparam name="TViewModelImpl">The type, that implements a view model.</typeparam>
        public static void InstallView<TView, TViewModel, TViewModelImpl>(this DiContainer container)
            where TView : ViewBehaviour<TViewModel>
            where TViewModel : class, IViewModel
            where TViewModelImpl : class, TViewModel
        {
            if (!ContainerEnvironments.TryGetValue(container, out var env))
            {
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                                                    $"Use {nameof(UseAsMvvmContainer)} method to configure container.");
            }
            env.Mapper.Map<TView, TViewModelImpl>();
        }

        /// <summary>
        /// Provides an access for specified objet types for view layer.
        /// </summary>
        /// <typeparam name="T">The type of dependency to be resolved in view layer.</typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use UseAsMvvmContainer to configure container.
        /// </exception>
        public static void ProvideAccessForViewLayer<T>(this DiContainer container)
        {
            if (!ContainerEnvironments.TryGetValue(container, out var env))
            {
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                                                    $"Use {nameof(UseAsMvvmContainer)} method to configure container.");
            }
            env.ViewsContainerAdapter.Container.Bind<T>().FromMethod(_ => container.Resolve<T>());
        }

        /// <summary>
        /// Provides an access for specified objet types for view layer.
        /// </summary>
        /// <typeparam name="TModelAccessInterface">How the dependency is bound in the model layer.</typeparam>
        /// <typeparam name="TCommonAccessInterface">How the dependency should be bound in the view layer.</typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use UseAsMvvmContainer method to configure container.
        /// </exception>
        public static void ProvideAccessForViewLayer<TModelAccessInterface, TCommonAccessInterface>(this DiContainer container)
        {
            if (!ContainerEnvironments.TryGetValue(container, out var env))
            {
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                                                    $"Use {nameof(UseAsMvvmContainer)} method to configure container.");
            }
            env.ViewsContainerAdapter.Container.Bind<TCommonAccessInterface>()
                .FromMethod(_ =>
                {
                    if (container.Resolve<TModelAccessInterface>() is TCommonAccessInterface common)
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
        /// Use UseAsMvvmContainer to configure container.
        /// </exception>
        public static void ProvideAccessForViewModelLayer<T>(this DiContainer container)
        {
            if (!ContainerEnvironments.TryGetValue(container, out var env))
            {
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                                                    $"Use {nameof(UseAsMvvmContainer)} method to configure container.");
            }
            env.ViewsModelsContainerAdapter.Container.Bind<T>().FromMethod(_ => container.Resolve<T>());
        }

        /// <summary>
        /// Provides an access for specified objet types for view-model layer.
        /// </summary>
        /// <typeparam name="TModelAccessInterface">How the dependency is bound in the model layer.</typeparam>
        /// <typeparam name="TCommonAccessInterface">How the dependency should be bound in the view-model layer.</typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use UseAsMvvmContainer to configure container.
        /// </exception>
        public static void ProvideAccessForViewModelLayer<TModelAccessInterface, TCommonAccessInterface>(this DiContainer container)
        {
            if (!ContainerEnvironments.TryGetValue(container, out var env))
            {
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                                                    $"Use {nameof(UseAsMvvmContainer)} method to configure container.");
            }
            env.ViewsModelsContainerAdapter.Container.Bind<TCommonAccessInterface>()
                .FromMethod(_ =>
                {
                    if (container.Resolve<TModelAccessInterface>() is TCommonAccessInterface common)
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
        /// <typeparam name="TImpl">The dependency implementation.</typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use UseAsMvvmContainer to configure container.
        /// </exception>
        public static void FastBind<TModelAccessInterface, TCommonAccessInterface, TImpl>(this DiContainer container)
        where TImpl : TCommonAccessInterface, TModelAccessInterface
        {
            container.FastBind<TImpl>(new []{typeof(TModelAccessInterface)} ,
                new []{typeof(TCommonAccessInterface)});
        }

        /// <summary>
        /// Binds a dependency for both model and view-model layers.
        /// </summary>
        /// <param name="container">MVVM container to configure.</param>
        /// <param name="modelAccessInterfaces">Interface to access the dependency in the model layer.</param>
        /// <param name="commonAccessInterfaces">Interface to access the dependency in view-model layer.</param>
        /// <typeparam name="TImpl">The dependency implementation.</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use UseAsMvvmContainer to configure container.
        /// </exception>
        public static void FastBind<TImpl>(this DiContainer container, IReadOnlyCollection<Type> modelAccessInterfaces,
            IReadOnlyCollection<Type> commonAccessInterfaces)
        {
            if (!ContainerEnvironments.TryGetValue(container, out var env))
            {
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                                                    $"Use {nameof(UseAsMvvmContainer)} method to configure container.");
            }
            var bindTypes = modelAccessInterfaces.Concat(commonAccessInterfaces).ToList();
            if (typeof(TImpl).Interfaces().Contains(typeof(IInitializable)))
            {
                bindTypes.Add(typeof(IInitializable));
            }
            container.Bind(bindTypes)
                .To<TImpl>().AsSingle();

            if (commonAccessInterfaces.IsEmpty())
            {
                commonAccessInterfaces = modelAccessInterfaces;
            }
            env.ViewsModelsContainerAdapter.Container.Bind(commonAccessInterfaces)
                .FromMethod<TImpl>(_ => (TImpl)container.Resolve(modelAccessInterfaces.First())).AsSingle();
        }

        /// <summary>
        /// Binds a dependency for both model and view-model layers.
        /// </summary>
        /// <typeparam name="TModelAccessInterface">The interface to access the dependency in the model layer.</typeparam>
        /// <typeparam name="TCommonAccessInterface">The interface to access the dependency in view-model layer.</typeparam>
        /// <typeparam name="TImpl">The implementation of the dependency</typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <param name="prefab">The dependency prefab.</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use UseAsMvvmContainer to configure container.
        /// </exception>
        public static void FastBindMono<TModelAccessInterface, TCommonAccessInterface, TImpl>(this DiContainer container,
            GameObject? prefab = null)
        where TImpl : MonoBehaviour, TCommonAccessInterface, TModelAccessInterface
        {
            container.FastBindMono<TImpl>(new []{typeof(TModelAccessInterface)},
                new []{typeof(TCommonAccessInterface)},
                prefab);
        }

        /// <summary>
        /// Binds a dependency for both model and view-model layers.
        /// </summary>
        /// <param name="container">MVVM container to configure.</param>
        /// <param name="modelAccessInterfaces">Interfaces to access the dependency in the model layer.</param>
        /// <param name="commonAccessInterfaces">Interfaces to access the dependency in view-model layer.</param>
        /// <param name="prefab">The dependency prefab.</param>
        /// <typeparam name="TImpl">The implementation of the dependency</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use UseAsMvvmContainer to configure container.
        /// </exception>
        public static void FastBindMono<TImpl>(this DiContainer container,
            IReadOnlyCollection<Type> modelAccessInterfaces,
            IReadOnlyCollection<Type> commonAccessInterfaces,
            GameObject? prefab = null)
        {
            if (!ContainerEnvironments.TryGetValue(container, out var env))
            {
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                                                    $"Use {nameof(UseAsMvvmContainer)} method to configure container.");
            }
            var bindTypes = modelAccessInterfaces.Concat(commonAccessInterfaces).ToList();
            if (typeof(TImpl).Interfaces().Contains(typeof(IInitializable)))
            {
                bindTypes.Add(typeof(IInitializable));
            }

            if (prefab == null)
            {
                container.Bind(bindTypes)
                    .To<TImpl>().FromNewComponentOnNewGameObject().AsSingle();
            }
            else
            {
                container.Bind(bindTypes)
                    .To<TImpl>().FromComponentInNewPrefab(prefab).AsSingle();
            }

            if (commonAccessInterfaces.IsEmpty())
            {
                commonAccessInterfaces = modelAccessInterfaces;
            }
            env.ViewsModelsContainerAdapter.Container.Bind(commonAccessInterfaces)
                .FromMethod<TImpl>(_ => (TImpl)container.Resolve(modelAccessInterfaces.First())).AsSingle();
        }

        /// <summary>
        /// Binds a dependency for both model and view model access layers.
        /// </summary>
        /// <typeparam name="TCommonAccessInterface">The interface to access the model in view-model layer.</typeparam>
        /// <typeparam name="TImpl">The implementation of the dependency</typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use UseAsMvvmContainer to configure container.
        /// </exception>
        public static void FastBind<TCommonAccessInterface, TImpl>(this DiContainer container)
        where TImpl : TCommonAccessInterface
        {
            container.FastBind<TImpl>(new [] {typeof(TCommonAccessInterface)});
        }

        /// <summary>
        /// Binds a dependency for both model and view model access layers.
        /// </summary>
        /// <param name="container">MVVM container to configure.</param>
        /// <param name="commonAccessInterfaces">Interfaces to access the model in view-model layer.</param>
        /// <typeparam name="TImpl">The implementation of the dependency</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use UseAsMvvmContainer to configure container.
        /// </exception>
        public static void FastBind<TImpl>(this DiContainer container,
            IReadOnlyCollection<Type> commonAccessInterfaces)
        {
            container.FastBind<TImpl>(commonAccessInterfaces, Array.Empty<Type>());
        }

        /// <summary>
        /// Binds a dependency for both model and view model access layers.
        /// </summary>
        /// <typeparam name="TCommonAccessInterface">The interface to access the model in view-model layer.</typeparam>
        /// <typeparam name="TImpl">The implementation of the dependency.</typeparam>
        /// <param name="container">MVVM container to configure.</param>
        /// <param name="prefab">The dependency prefab</param>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use UseAsMvvmContainer to configure container.
        /// </exception>
        public static void FastBindMono<TCommonAccessInterface, TImpl>(this DiContainer container, GameObject? prefab = null)
            where TImpl : MonoBehaviour, TCommonAccessInterface
        {
            container.FastBindMono<TImpl>(new []{typeof(TCommonAccessInterface)}, prefab);
        }

        /// <summary>
        /// Binds a dependency for both model and view model access layers.
        /// </summary>
        /// <param name="container">MVVM container to configure.</param>
        /// <param name="commonAccessInterfaces">Interfaces to access the model in view-model layer.</param>
        /// <param name="prefab">The dependency prefab</param>
        /// <typeparam name="TImpl">The implementation of the dependency.</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Being thrown if it is not MVVM container.
        /// Use UseAsMvvmContainer to configure container.
        /// </exception>
        public static void FastBindMono<TImpl>(this DiContainer container,
            IReadOnlyCollection<Type> commonAccessInterfaces,
            GameObject? prefab = null)
        {
            container.FastBindMono<TImpl>(
                commonAccessInterfaces, 
                Array.Empty<Type>(),
                prefab);
        }

        /// <summary>
        /// Returns a container for view-model layer.
        /// </summary>
        /// <param name="container">The MVVM container.</param>
        public static DiContainer GetViewModelsContainer(this DiContainer container)
        {
            if (!ContainerEnvironments.TryGetValue(container, out var env))
            {
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                                                    $"Use {nameof(UseAsMvvmContainer)} method to configure container.");
            }
            return env.ViewsModelsContainerAdapter.Container;
        }

        /// <summary>
        /// Returns a container for view layer.
        /// </summary>
        /// <param name="container">The MVVM container.</param>
        public static DiContainer GetViewsContainer(this DiContainer container)
        {
            if (!ContainerEnvironments.TryGetValue(container, out var env))
            {
                throw new InvalidOperationException($"Provided container does not contain container for the view-model layer. " +
                                                    $"Use {nameof(UseAsMvvmContainer)} method to configure container.");
            }
            return env.ViewsContainerAdapter.Container;
        }
    }
}