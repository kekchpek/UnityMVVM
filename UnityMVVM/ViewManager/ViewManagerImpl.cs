using System;
using System.Collections.Generic;
using System.Linq;
using UnityMVVM.DI;
using UnityMVVM.ViewManager.ViewLayer;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager
{
    /// <inheritdoc cref="IViewManager"/>
    public class ViewManagerImpl : IViewManager
    {

        private readonly IViewLayer[] _layers;
        private readonly IViewsContainerAdapter _viewsContainer;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="layers">Layers to place views.</param>
        /// <param name="viewsContainerAdapter">Adapter for views DI container.</param>
        public ViewManagerImpl(IEnumerable<IViewLayer> layers, IViewsContainerAdapter viewsContainerAdapter)
        {
            _layers = layers.ToArray();
            _viewsContainer = viewsContainerAdapter;
        }

        /// <summary>
        /// Closes all views on specified layer.
        /// </summary>
        /// <param name="viewLayerId">Id of layer to clear.</param>
        public void Close(string viewLayerId)
        {
            _layers.First(l => l.Id == viewLayerId).Clear();
        }

        T IViewManager.Create<T>(IViewModel parent)
        {
            return _viewsContainer.ResolveFactory<T>().Create(parent.Layer.Container, parent);
        }

        void IViewManager.Open<T>(string viewLayerId)
        {
            for(int i = _layers.Length - 1; i >= 0; i--)
            {
                _layers[i].Clear();
                if (_layers[i].Id == viewLayerId)
                {
                    var viewModel = _viewsContainer.ResolveFactory<T>().Create(_layers[i].Container, null);
                    _layers[i].Set(viewModel);
                    break;
                }
                if (i == 0)
                {
                    throw new InvalidOperationException($"Can not find view layer with id = {viewLayerId}");
                }
            }
        }
    }
}
