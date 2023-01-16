using System;
using System.Collections.Generic;
using System.Linq;
using UnityMVVM.DI;
using UnityMVVM.ViewManager.ViewLayer;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager
{
    public class ViewManagerImpl : IViewManager
    {

        private readonly IViewLayer[] _layers;
        private readonly IViewsContainer _viewsContainer;

        public ViewManagerImpl(IEnumerable<IViewLayer> layers, IViewsContainer viewsContainer)
        {
            _layers = layers.ToArray();
            _viewsContainer = viewsContainer;
        }

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
