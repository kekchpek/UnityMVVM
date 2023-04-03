using AsyncReactAwait.Promises;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="layers">Layers to place views.</param>
        /// <param name="viewsContainerAdapter">Adapter for views DI container.</param>
        public ViewManagerImpl(IEnumerable<IViewLayer> layers, IViewsModelsContainerAdapter viewsContainerAdapter)
        {
            _layers = layers.ToArray();
            _viewsContainer = viewsContainerAdapter;
        }


        /// <inheritdoc cref="IViewManager.Close(string)"/>
        public async IPromise Close(string viewLayerId)
        {
            await _layers.First(l => l.Id == viewLayerId).Clear();
        }

        /// <inheritdoc />
        public async IPromise CloseAbove(string viewLayerId)
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

        /// <inheritdoc cref="IViewManager.Create{T}(IViewModel, string, IPayload)"/>
        public T Create<T>(IViewModel parent, string viewName, IPayload? payload = null)
             where T : class, IViewModel
        {
            var viewModel = Create(parent, viewName, payload);
            if (viewModel is T concreteViewModel)
                return concreteViewModel;
            throw new InvalidCastException($"Can not cast view model of type {viewModel.GetType().Name} to {typeof(T).Name}");
        }

        public IViewModel Create(IViewModel parent, string viewName, IPayload? payload = null)
        {
            return _viewsContainer.ResolveViewFactory(viewName).Create(parent.Layer, parent, payload);
        }

        /// <inheritdoc cref="IViewManager.Open(string, string, IPayload)"/>
        public async IPromise Open(string viewLayerId, string viewName, IPayload? payload = null)
        {
            if (_openingLayer == viewLayerId)
            {
                Debug.LogError("Attempt to open view while other one is being opened.");
                return;
            }
            _openingLayer = viewLayerId;
            try
            {
                for (int i = _layers.Length - 1; i >= -1; i--)
                {
                    if (i == -1)
                    {
                        throw new InvalidOperationException($"Can not find view layer with id = {viewLayerId}");
                    }

                    await _layers[i].Clear();
                    if (_layers[i].Id == viewLayerId)
                    {
                        var viewModel = _viewsContainer.ResolveViewFactory(viewName).Create(_layers[i], null, payload);
                        _createdViewsNames.Add(viewModel, viewName);
                        void OnViewModelDestroyed()
                        {
                            viewModel.Destroyed -= OnViewModelDestroyed;
                            _createdViewsNames.Remove(viewModel);
                        }
                        viewModel.Destroyed += OnViewModelDestroyed;
                        _layers[i].Set(viewModel);
                        break;
                    }
                }
            }
            finally
            {
                _openingLayer = null;
            }

        }
    }
}
