using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using UnityEngine.Scripting;
using UnityMVVM.DI;
using UnityMVVM.DI.Mapper;
using UnityMVVM.Pool;
using UnityMVVM.ViewManager.ViewLayer;
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
        private readonly Func<GameObject> _viewPrefabGetter;
        private readonly IViewPool? _viewPool;

        /// <summary>
        /// Default constructor for view factory.
        /// </summary>
        /// <param name="viewPrefabGetter">The method to obtain prefab of the view.</param>
        /// <param name="instantiator">Instantiator for view models.</param>
        /// <param name="viewToViewModelMapper">Map of views and view models types.</param>
        /// <param name="viewFactory">The view factory to create and initialize views.</param>
        /// <param name="viewPool">The pool for views(if presented)</param>
        [Preserve]
        public ViewModelsFactory( 
            Func<GameObject> viewPrefabGetter,
            IInstantiator instantiator,
            IViewToViewModelMapper viewToViewModelMapper, 
            IViewFactory viewFactory,
            IViewPool viewPool)
        {
            _viewPrefabGetter = viewPrefabGetter;
            _instantiator = instantiator;
            _viewToViewModelMapper = viewToViewModelMapper;
            _viewFactory = viewFactory;
            _viewPool = viewPool;
        }

        /// <inheritdoc cref="IViewModelsFactory.Create(IViewLayer, IViewModel, Transform, IPayload)"/>
        public IViewModelInternal Create(IViewLayer viewLayer,
            IViewModel? parent,
            Transform transform,
            IPayload? payload = null)
        {
            // TODO this should be moved to other entity
            TView view = _viewFactory.Instantiate<TView>(_viewPrefabGetter.Invoke(), transform, _viewPool);

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

        private IViewModelInternal CreateViewModels(
            Transform initialObj,
            IViewLayer layer, 
            IViewModel? initialParent, 
            IPayload? payload,
            bool isPoolableView)
        {
            Queue<(Transform obj, IViewModel? parent)> creationQueue = new Queue<(Transform obj, IViewModel? parent)>();
            creationQueue.Enqueue((initialObj, initialParent));
            IViewModelInternal? rootViewModel = null;
            while (creationQueue.Count > 0)
            {
                var data = creationQueue.Dequeue();
                var parent = data.parent;
                var view = data.obj.GetComponent<IViewBehaviour>();
                if (view != null)
                {
                    var viewModelType = _viewToViewModelMapper.GetViewModelForView(view.GetType());
                    var implicitParams = new List<object>(3);
                    if (data.parent != null)
                    {
                        implicitParams.Add(data.parent);
                    }
                    if (payload != null 
                        && viewModelType.Constructors().Any(x => x.GetParameters().Any(ctor => ctor.ParameterType.IsInstanceOfType(payload))))
                    {
                        implicitParams.Add(payload);
                    }
                    implicitParams.Add(layer);
                    var viewModel = _instantiator.Instantiate(viewModelType, implicitParams);
                    if (viewModel is IInitializable initializable)
                    {
                        initializable.Initialize();
                    }
                    
                    _viewFactory.Initialize((IViewInitializer)view, (IViewModel)viewModel, isPoolableView);
                    
                    rootViewModel ??= (IViewModelInternal)viewModel;
                    parent = (IViewModelInternal)viewModel;
                }
                foreach (Transform child in data.obj)
                {
                    creationQueue.Enqueue((child, parent));
                }
            }
            if (rootViewModel == null)
            {
                throw new Exception("No view models were created during view instantiation.");
            }
            return rootViewModel;
        }
    }
}