using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.DI;
using UnityMVVM.DI.Mapper;
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

        /// <summary>
        /// Default constructor for view factory.
        /// </summary>
        /// <param name="viewsContainerAdapter"></param>
        /// <param name="viewPrefabGetter">The method to obtain prefab of the view.</param>
        /// <param name="instantiator">Instantiator for view models.</param>
        /// <param name="viewToViewModelMapper">Map of views and view models types.</param>
        public ViewFactory( 
            IViewsContainerAdapter viewsContainerAdapter, 
            Func<GameObject> viewPrefabGetter,
            IInstantiator instantiator,
            IViewToViewModelMapper viewToViewModelMapper)
        {
            _viewsContainerAdapter = viewsContainerAdapter;
            _viewPrefabGetter = viewPrefabGetter;
            _instantiator = instantiator;
            _viewToViewModelMapper = viewToViewModelMapper;
        }

        /// <inheritdoc cref="IViewFactory.Create(IViewLayer, IViewModel, IPayload)"/>
        public IViewModelInternal Create(IViewLayer viewLayer,
            IViewModel? parent,
            Transform transform,
            IPayload? payload = null)
        {

            var view = _viewsContainerAdapter.Container.InstantiatePrefabForComponent<TView>(_viewPrefabGetter.Invoke(), transform);

            if (view is Component c)
            {
                var rootViewModel = CreateViewModels(c.transform, viewLayer, parent, payload);
                return rootViewModel;
            }

            throw new Exception("View should be a Component");
        }

        private IViewModelInternal CreateViewModels(
            Transform initialObj,
            IViewLayer layer, 
            IViewModel? initialParent, 
            IPayload? payload)
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
                    var implicitParams = new List<object>(3);
                    if (data.parent != null)
                    {
                        implicitParams.Add(data.parent);
                    }
                    if (payload != null)
                    {
                        implicitParams.Add(payload);
                    }
                    implicitParams.Add(layer);
                    var viewModel = _instantiator.Instantiate(_viewToViewModelMapper.GetViewModelForView(view.GetType()), implicitParams);
                    if (viewModel is IInitializable initializable)
                    {
                        initializable.Initialize();
                    }
                    ((IViewInitializer)view).SetViewModel((IViewModel)viewModel);
                    if (rootViewModel == null)
                    {
                        rootViewModel = (IViewModelInternal)viewModel;
                    }
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