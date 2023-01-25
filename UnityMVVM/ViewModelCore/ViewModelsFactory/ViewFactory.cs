using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityMVVM.DI;
using UnityMVVM.DI.Mapper;
using UnityMVVM.ViewManager.ViewLayer;
using Zenject;

namespace UnityMVVM.ViewModelCore.ViewModelsFactory
{

    /// <inheritdoc cref="IViewFactory"/>
    internal class ViewFactory<TView, TViewModel> : IViewFactory
        where TView : IViewInitializer<TViewModel>
        where TViewModel : IViewModel
    {
        private readonly IViewsContainerAdapter _viewsContainerAdapter;
        private readonly IInstantiator _instantiator;
        private readonly IViewToViewModelMapper _viewToViewModelMapper;
        private readonly GameObject _viewPrefab;

        /// <summary>
        /// Default constructor for view factory.
        /// </summary>
        /// <param name="viewsContainerAdapter"></param>
        /// <param name="viewPrefab">Prefab of the view.</param>
        /// <param name="instantiator">Instantiator for view models.</param>
        /// <param name="viewToViewModelMapper">Map of views and view models types.</param>
        public ViewFactory( 
            IViewsContainerAdapter viewsContainerAdapter, 
            GameObject viewPrefab,
            IInstantiator instantiator,
            IViewToViewModelMapper viewToViewModelMapper)
        {
            _viewsContainerAdapter = viewsContainerAdapter;
            _viewPrefab = viewPrefab;
            _instantiator = instantiator;
            _viewToViewModelMapper = viewToViewModelMapper;
        }

        /// <inheritdoc cref="IViewFactory.Create(IViewLayer, IViewModel, IPayload)"/>
        public IViewModel Create(IViewLayer viewLayer,
            [CanBeNull, AllowNull] IViewModel parent, 
            [CanBeNull, AllowNull] IPayload payload = null)
        {

            var view = _viewsContainerAdapter.Container.InstantiatePrefabForComponent<TView>(_viewPrefab, viewLayer.Container);

            if (view is Component c)
            {
                var rootViewModel = CreateViewModels(c.transform, viewLayer, parent, payload);

                view.SetViewModel((TViewModel)rootViewModel);
                return rootViewModel;
            }

            throw new Exception("View should be a Component");
        }

        private IViewModel CreateViewModels(
            Transform initialObj,
            IViewLayer layer, 
            [CanBeNull, AllowNull] IViewModel initialParent, 
            [CanBeNull, AllowNull] IPayload payload)
        {
            Queue<(Transform obj, IViewModel? parent)> creationQueue = new Queue<(Transform obj, IViewModel? parent)>();
            creationQueue.Enqueue((initialObj, initialParent));
            IViewModel? rootViewModel = null;
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
                    if (rootViewModel == null)
                    {
                        rootViewModel = (IViewModel)viewModel;
                    }
                    parent = data.parent;
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