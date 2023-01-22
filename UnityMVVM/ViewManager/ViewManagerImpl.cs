﻿using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityMVVM.DI;
using UnityMVVM.ViewManager.ViewLayer;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager
{
    /// <inheritdoc cref="IViewManager"/>
    internal class ViewManagerImpl : IViewManager
    {

        private readonly IViewLayer[] _layers;
        private readonly IViewsModelsContainerAdapter _viewsContainer;

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
        public void Close(string viewLayerId)
        {
            _layers.First(l => l.Id == viewLayerId).Clear();
        }

        /// <inheritdoc cref="IViewManager.Create{T}(IViewModel, string, IPayload)"/>
        public T Create<T>(IViewModel parent, string viewName, [AllowNull, CanBeNull] IPayload payload = null)
             where T : class, IViewModel
        {
            return _viewsContainer.ResolveFactory<T>(viewName).Create(parent.Layer, parent, payload);
        }

        public IViewModel Create(IViewModel parent, string viewName, [AllowNull, CanBeNull] IPayload payload = null)
        {
            return Create<IViewModel>(parent, viewName, payload);
        }

        /// <inheritdoc cref="IViewManager.Open(string, string, IPayload)"/>
        public void Open<T>(string viewLayerId, string viewName, [AllowNull, CanBeNull] IPayload payload = null)
             where T : class, IViewModel
        {
            for(int i = _layers.Length - 1; i >= 0; i--)
            {
                _layers[i].Clear();
                if (_layers[i].Id == viewLayerId)
                {
                    var viewModel = _viewsContainer.ResolveFactory<T>(viewName).Create(_layers[i], null, payload);
                    _layers[i].Set(viewModel);
                    break;
                }
                if (i == -1)
                {
                    throw new InvalidOperationException($"Can not find view layer with id = {viewLayerId}");
                }
            }
        }

        public void Open(string viewLayerId, string viewName, [AllowNull, CanBeNull] IPayload payload = null)
        {
            Open<IViewModel>(viewLayerId, viewName, payload);
        }
    }
}
