using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using UnityEngine.Scripting;
using UnityMVVM.DI.Mapper;
using UnityMVVM.Pool;
using UnityMVVM.ViewManager.ViewLayer;
using UnityMVVM.ViewModelCore.PrefabsProvider;
using Zenject;

namespace UnityMVVM.ViewModelCore.ViewModelsFactory
{

    /// <inheritdoc cref="IViewModelsFactory"/>
    internal class ViewModelsFactory<TView> : IViewModelsFactory
        where TView : IViewInitializer
    {
        
        private readonly IInstantiator _instantiator;
        private readonly IViewToViewModelMapper _viewToViewModelMapper;
        private readonly IViewFactory _viewFactory;
        private readonly Func<string, GameObject>? _viewPrefabGetter;
        private readonly IViewPool? _viewPool;
        private readonly IViewsPrefabsProvider? _viewsPrefabsProvider;

        /// <summary>
        /// Default constructor for view factory.
        /// </summary>
        /// <param name="viewPrefabGetter">The method to obtain prefab of the view.</param>
        /// <param name="instantiator">Instantiator for view models.</param>
        /// <param name="viewToViewModelMapper">Map of views and view models types.</param>
        /// <param name="viewFactory">The view factory to create and initialize views.</param>
        /// <param name="viewPool">The pool for views(if presented)</param>
        /// <param name="viewsPrefabsProvider">The default provider for views prefabs.</param>
        [Preserve]
        public ViewModelsFactory( 
            Func<string, GameObject>? viewPrefabGetter,
            IInstantiator instantiator,
            IViewToViewModelMapper viewToViewModelMapper, 
            IViewFactory viewFactory,
            IViewPool viewPool,
            [InjectOptional] IViewsPrefabsProvider viewsPrefabsProvider)
        {
            _viewPrefabGetter = viewPrefabGetter;
            _instantiator = instantiator;
            _viewToViewModelMapper = viewToViewModelMapper;
            _viewFactory = viewFactory;
            _viewPool = viewPool;
            _viewsPrefabsProvider = viewsPrefabsProvider;
        }

        /// <inheritdoc cref="IViewModelsFactory.Create(IViewLayer, string, IViewModel, Transform, IPayload)"/>
        public IViewModel Create(IViewLayer viewLayer,
            string viewName,
            IViewModel? parent,
            Transform transform,
            IPayload? payload = null)
        {
            GameObject viewPrefab;
            if (_viewPrefabGetter != null)
            {
                viewPrefab = _viewPrefabGetter.Invoke(viewName);
            }
            else
            {
                if (_viewsPrefabsProvider != null)
                {
                    viewPrefab = _viewsPrefabsProvider!.GetViewPrefab(viewName);
                }
                else
                {
                    throw new Exception(
                        "There should be either getter for prefab or default views prefabs provider bound.");
                }
            }
            var view = _viewFactory.Instantiate<TView>(viewPrefab, transform, _viewPool);

            if (view is not Component c)
                throw new Exception("View should be a Component");
            
            var rootViewModel = CreateViewModels(
                c.transform, 
                viewLayer, 
                parent, 
                payload, 
                _viewPool != null);
            return rootViewModel;

        }

        private IViewModel CreateViewModels(
            Transform initialObj,
            IViewLayer layer, 
            IViewModel? initialParent, 
            IPayload? payload,
            bool isPoolableView)
        {
            
            // Get hierarchy depth and width to approximately determine amount of memory
            // required for handling all child objects
            var hierarchyDepth = 1;
            var maxWidth = 0;
            var t = initialObj;
            int childrenCount;
            while ((childrenCount = t.childCount) > 0)
            {
                maxWidth = Math.Max(childrenCount, maxWidth);
                t = t.GetChild(0);
                hierarchyDepth++;
            }
            
            // Create a queue for handling children without recursion
            var creationQueue = new Queue<(Transform obj, IViewModel? parent)>(hierarchyDepth * maxWidth);
            
            // add root object to the queue
            creationQueue.Enqueue((initialObj, initialParent));
            IViewModel? rootViewModel = null;

            Stack<IViewModel> setupStack = new Stack<IViewModel>();

            // Iterate through entire hierarchy to create view-model for every view component
            while (creationQueue.Count > 0)
            {
                // Pop object data from queue
                var data = creationQueue.Dequeue();
                var parent = data.parent;
                
                // Skip if there is no view on the object
                if (data.obj.TryGetComponent<IViewBehaviour>(out var view))
                {
                    var viewModelType = _viewToViewModelMapper.GetViewModelForView(view.GetType());
                    var implicitParams = new List<object>(3);
                    if (data.parent != null)
                    {
                        implicitParams.Add(data.parent);
                    }
                    if (payload != null && DoesHavePayload(viewModelType, payload))
                    {
                        implicitParams.Add(payload);
                    }
                    implicitParams.Add(layer);
                    var viewModel = (IViewModel)_instantiator.Instantiate(viewModelType, implicitParams);
                    setupStack.Push(viewModel);
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    if (viewModel is IInitializable initializable)
                    {
                        initializable.Initialize();
                    }
                    data.parent?.AddSubview(viewModel);

                    if (view is IViewInitializer initializer)
                    {
                        viewModel.SetId(initializer.GetViewId());
                        _viewFactory.Initialize(initializer, viewModel, isPoolableView);
                    }
                    else 
                        throw new InvalidCastException($"All view types should be able to be casted to {nameof(IViewInitializer)}");
                    
                    // The first create view-modes becomes root view model for this instantiation.
                    rootViewModel ??= viewModel;
                    
                    // Rewrite parent for views, that are placed under this view in hierarchy
                    parent = viewModel;
                }
                
                // Add children to be handled in next iterations
                for (var i = 0; i < data.obj.childCount; i++)
                {
                    creationQueue.Enqueue((data.obj.GetChild(i), parent));
                }
            }

            while (setupStack.TryPop(out var vm))
            {
                vm.SetupCompleted();
            }
            
            if (rootViewModel == null)
            {
                throw new Exception("No view models were created during view instantiation.");
            }
            return rootViewModel;
        }

        private static bool DoesHavePayload(Type viewModelType, IPayload payload)
        {
            foreach (var ctor in viewModelType.Constructors())
            {
                foreach (var parameter in ctor.GetParameters())
                {
                    if (parameter.ParameterType.IsInstanceOfType(payload))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}