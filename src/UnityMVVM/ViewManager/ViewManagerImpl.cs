using AsyncReactAwait.Promises;
using System;
using System.Collections.Generic;
using AsyncReactAwait.Bindable;
using UnityEngine;
using UnityEngine.Scripting;
using UnityMVVM.DI;
using UnityMVVM.ViewManager.ViewLayer;
using UnityMVVM.ViewModelCore;
// ReSharper disable All

namespace UnityMVVM.ViewManager
{
    /// <inheritdoc cref="IViewManager"/>
    internal class ViewManagerImpl : IViewManager
    {

        private const string UnknownViewName = "<UNKNOWN>";

        private readonly IReadOnlyList<IViewLayer> _layers;
        private readonly IReadOnlyList<string> _layerIds;
        private readonly IViewsModelsContainerAdapter _viewsContainer;

        private readonly IDictionary<IViewModel, string> _createdViewsNames = new Dictionary<IViewModel, string>();

        private string? _openingLayer;

        private readonly IMutable<string?> _highestBusyLayer = new Mutable<string?>();

        public event Action<(string layerId, string viewName, IPayload? viewPayload)>? ViewOpened;
        public event Action<(string layerId, string viewName)>? ViewClosedImplicitly;

        /// <inheritdoc />
        public IBindable<string?> HighestBusyLayer => _highestBusyLayer;

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
            _layers = System.Linq.Enumerable.ToArray(layers);
            var layerIds = new string[_layers.Count];
            for (var i = 0; i < _layers.Count; i++)
            {
                layerIds[i] = _layers[i].Id;
            }
            _layerIds = layerIds;

            _viewsContainer = viewsContainerAdapter;
            foreach (var viewLayer in _layers)
            {
                viewLayer.CurrentView.Bind(OnLayerViewChanged);
            }
        }

        private void OnLayerViewChanged()
        {
            UpdateHighestViewLayer();
        }

        private void UpdateHighestViewLayer()
        {
            string? highestLayerId = null;
            foreach (var l in _layers)
            {
                if (l.CurrentView.Value != null)
                {
                    highestLayerId = l.Id;
                }
            }

            _highestBusyLayer.Value = highestLayerId;
        }

        public async IPromise OpenExact(string viewLayerId, string viewName, IPayload? payload = null)
        {
            var layer = GetLayer(viewLayerId);

            await layer.Clear();
            CreateViewOnLayer(viewName, layer, payload);
        }

        /// <inheritdoc cref="IViewManager.CloseExact(string)"/>
        public async IPromise CloseExact(string viewLayerId)
        {
            await GetLayer(viewLayerId).Clear();
        }

        /// <inheritdoc cref="IViewManager.Close(string)"/>
        public async IPromise Close(string viewLayerId)
        {
            for (var i = _layers.Count - 1;;i--)
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
        public IViewLayer GetLayer(string viewLayerId)
        {
            foreach (var l in _layers)
            {
                if (l.Id == viewLayerId)
                    return l;
            }

            throw new InvalidOperationException($"Layer with id={viewLayerId} is not found.");
        }

        /// <inheritdoc />
        public IViewModel? GetView(string viewLayerId)
        {
            return GetLayer(viewLayerId).CurrentView.Value;
        }

        public IReadOnlyList<string> GetLayerIds()
        {
            return _layerIds;
        }

        public IViewModel Create(IViewModel parent, string viewName, Transform container, IPayload? payload = null)
        {
            return _viewsContainer.ResolveViewFactory(viewName).Create(parent.Layer, viewName, parent, container, payload);
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
                for (int i = _layers.Count - 1; i >= 0; i--)
                {
                    // close opened view
                    var openedViewModel = _layers[i].CurrentView.Value;
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
            var viewModel = _viewsContainer.ResolveViewFactory(viewName).Create(layer, viewName, null, layer.Container, payload);
            _createdViewsNames.Add(viewModel, viewName);
            viewModel.Destroyed += OnViewModelDestroyed;
            layer.Set(viewModel);
            viewModel.OnOpened();
            ViewOpened?.Invoke((layer.Id, viewName, payload));
            return viewModel;
        }

        private void OnViewModelDestroyed(IViewModel viewModel)
        {
            viewModel.Destroyed -= OnViewModelDestroyed;
            _createdViewsNames.Remove(viewModel);
        }
    }
}
