using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using UnityMVVM.DI;
using UnityMVVM.DI.Mapper;
using UnityMVVM.Pool;
using UnityMVVM.ViewManager.ViewLayer;
using Zenject;

namespace UnityMVVM.ViewModelCore.ViewModelsFactory
{

    /// <inheritdoc cref="IViewFactory"/>
    internal class ViewFactory<TView> : IViewFactory
        where TView : IViewInitializer
    {
        private readonly IViewsContainerAdapter _viewsContainerAdapter;
        private readonly IInstantiator _instantiator;
        private readonly IViewToViewModelMapper _viewToViewModelMapper;
        private readonly Func<GameObject> _viewPrefabGetter;
        private readonly IViewPool? _viewPool;

        /// <summary>
        /// Default constructor for view factory.
        /// </summary>
        /// <param name="viewsContainerAdapter"></param>
        /// <param name="viewPrefabGetter">The method to obtain prefab of the view.</param>
        /// <param name="instantiator">Instantiator for view models.</param>
        /// <param name="viewToViewModelMapper">Map of views and view models types.</param>
        /// <param name="viewPool">The pool for views(if presented)</param>
        public ViewFactory( 
            IViewsContainerAdapter viewsContainerAdapter, 
            Func<GameObject> viewPrefabGetter,
            IInstantiator instantiator,
            IViewToViewModelMapper viewToViewModelMapper, 
            IViewPool viewPool)
        {
            _viewsContainerAdapter = viewsContainerAdapter;
            _viewPrefabGetter = viewPrefabGetter;
            _instantiator = instantiator;
            _viewToViewModelMapper = viewToViewModelMapper;
            _viewPool = viewPool;
        }

        /// <inheritdoc cref="IViewFactory.Create(IViewLayer, IViewModel, Transform, IPayload)"/>
        public IViewModelInternal Create(IViewLayer viewLayer,
            IViewModel? parent,
            Transform transform,
            IPayload? payload = null)
        {
            TView view;
            if (_viewPool != null && _viewPool.TryPop(out var poolableView))
            {
                view = (TView)poolableView!;
                view.SetParent(transform);
            }
            else
            {
                view = _viewsContainerAdapter.Container.InstantiatePrefabForComponent<TView>(
                    _viewPrefabGetter.Invoke(), transform);
                view.SetPool(_viewPool);
            }

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
                    ((IViewInitializer)view).SetViewModel((IViewModel)viewModel);
                    ((IViewInitializer)view).SetPartOfPoolableView(isPoolableView);
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