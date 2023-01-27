using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityAuxiliaryTools.Promises;
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
        public async IPromise Close(string viewLayerId)
        {
            await _layers.First(l => l.Id == viewLayerId).Clear();
        }

        /// <inheritdoc cref="IViewManager.Create{T}(IViewModel, string, IPayload)"/>
        public T Create<T>(IViewModel parent, string viewName, [AllowNull, CanBeNull] IPayload payload = null)
             where T : class, IViewModel
        {
            var viewModel = Create<IViewModel>(parent, viewName, payload);
            if (viewModel is T concreteViewModel)
                return concreteViewModel;
            throw new InvalidCastException($"Can not cast view model of type {viewModel.GetType().Name} to {typeof(T).Name}");
        }

        public IViewModel Create(IViewModel parent, string viewName, [AllowNull, CanBeNull] IPayload payload = null)
        {
             return _viewsContainer.ResolveViewFactory(viewName).Create(parent.Layer, parent, payload);
        }

        /// <inheritdoc cref="IViewManager.Open(string, string, IPayload)"/>
        public async IPromise Open<T>(string viewLayerId, string viewName, [AllowNull, CanBeNull] IPayload payload = null)
             where T : class, IViewModel
        {
            for(int i = _layers.Length - 1; i >= 0; i--)
            {
                await _layers[i].Clear();
                if (_layers[i].Id == viewLayerId)
                {
                    var viewModel = _viewsContainer.ResolveViewFactory(viewName).Create(_layers[i], null, payload);
                    _layers[i].Set(viewModel);
                    break;
                }
                if (i == -1)
                {
                    throw new InvalidOperationException($"Can not find view layer with id = {viewLayerId}");
                }
            }
        }

        public async IPromise Open(string viewLayerId, string viewName, [AllowNull, CanBeNull] IPayload payload = null)
        {
            await Open<IViewModel>(viewLayerId, viewName, payload);
        }
    }
}
