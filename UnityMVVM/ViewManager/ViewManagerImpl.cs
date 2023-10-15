using AsyncReactAwait.Promises;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using UnityMVVM.DI;
using UnityMVVM.ViewManager.ViewLayer;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager
{
    /// <inheritdoc cref="IViewManager"/>
    internal class ViewManagerImpl : IViewManager
    {

        private const string UnknownViewName = "<UNKNOWN>";

        private readonly IViewLayer[] _layers;
        private readonly IViewsModelsContainerAdapter _viewsContainer;

        private readonly IDictionary<IViewModel, string> _createdViewsNames = new Dictionary<IViewModel, string>();

        private string? _openingLayer;

        public event Action<(string layerId, string viewName, IPayload? viewPayload)>? ViewOpened;
        public event Action<(string layerId, string viewName)>? ViewClosedImplicitly;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="layers">Layers to place views.</param>
        /// <param name="viewsContainerAdapter">Adapter for views DI container.</param>
        [Preserve]
        public ViewManagerImpl(
            IEnumerable<IViewLayer> layers, 
            IViewsModelsContainerAdapter viewsContainerAdapter)
        {
            _layers = layers.ToArray();
            _viewsContainer = viewsContainerAdapter;
        }


        public async IPromise OpenExact(string viewLayerId, string viewName, IPayload? payload = null)
        {
            var layer = _layers.FirstOrDefault(x => x.Id == viewLayerId);
            if (layer == null)
            {
                throw new Exception($"Can not find layer with id = {viewLayerId}");
            }

            await layer.Clear();
            CreateViewOnLayer(viewName, layer, payload);
        }

        /// <inheritdoc cref="IViewManager.CloseExact(string)"/>
        public async IPromise CloseExact(string viewLayerId)
        {
            await _layers.First(l => l.Id == viewLayerId).Clear();
        }

        /// <inheritdoc cref="IViewManager.Close(string)"/>
        public async IPromise Close(string viewLayerId)
        {
            for (var i = _layers.Length - 1;;i--)
            {
                await _layers[i].Clear();
                if (_layers[i].Id == viewLayerId)
                {
                    break;
                }
            }
        }

        /// <inheritdoc />
        public string? GetViewName(string viewLayerId)
        {
            var viewModel = GetView(viewLayerId);
            if (viewModel == null)
            {
                return null;
            }

            if (_createdViewsNames.ContainsKey(viewModel))
            {
                return _createdViewsNames[viewModel];
            }

            return UnknownViewName;
        }

        /// <inheritdoc />
        public IViewModel? GetView(string viewLayerId)
        {
            return _layers.First(x => x.Id == viewLayerId).GetCurrentView();
        }

        public string[] GetLayerIds()
        {
            return _layers.Select(x => x.Id).ToArray();
        }

        public IViewModel Create(IViewModel parent, string viewName, Transform container, IPayload? payload = null)
        {
            return _viewsContainer.ResolveViewFactory(viewName).Create(parent.Layer, parent, container, payload);
        }

        /// <inheritdoc cref="IViewManager.Open(string, string, IPayload)"/>
        public async IPromise<IViewModel?> Open(string viewLayerId, string viewName, IPayload? payload = null)
        {
            if (_openingLayer == viewLayerId)
            {
                Debug.LogError("Attempt to open view while other one is being opened.");
                return null;
            }
            _openingLayer = viewLayerId;
            try
            {
                for (int i = _layers.Length - 1; i >= 0; i--)
                {
                    // close opened view
                    var openedViewModel = _layers[i].GetCurrentView();
                    string? openedViewName = null;
                    if (openedViewModel!= null)
                        openedViewName = _createdViewsNames[openedViewModel];
                    await _layers[i].Clear();
                    if (openedViewName != null) 
                        ViewClosedImplicitly?.Invoke((_layers[i].Id, openedViewName));
                    
                    // open required view
                    if (_layers[i].Id == viewLayerId)
                    {
                        var viewModel = CreateViewOnLayer(viewName, _layers[i], payload);
                        return viewModel;
                    }
                }
                throw new InvalidOperationException($"Can not find view layer with id = {viewLayerId}");
            }
            finally
            {
                _openingLayer = null;
            }
        }

        private IViewModel CreateViewOnLayer(string viewName, IViewLayer layer, IPayload? payload)
        {
            
            var viewModel = _viewsContainer.ResolveViewFactory(viewName).Create(layer, null, layer.Container, payload);
            _createdViewsNames.Add(viewModel, viewName);
            void OnViewModelDestroyed()
            {
                viewModel.Destroyed -= OnViewModelDestroyed;
                _createdViewsNames.Remove(viewModel);
            }
            viewModel.Destroyed += OnViewModelDestroyed;
            layer.Set(viewModel);
            viewModel.OnOpened();
            ViewOpened?.Invoke((layer.Id, viewName, payload));
            return viewModel;
        }
    }
}
