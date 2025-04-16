using System;
using System.Collections.Generic;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Promises;
using UnityEngine;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewManager.ViewLayer;
using UnityMVVM.ViewModelCore;

namespace CCG.Core.CustomViewManager
{
    public class LogViewManagerDecorator : IViewManager
    {
        
        public event Action<(string layerId, string viewName, IPayload viewPayload)> ViewOpened
        {
            add => _viewManager.ViewOpened += value;
            remove => _viewManager.ViewOpened -= value;
        }
        
        public event Action<(string layerId, string viewName)> ViewClosedImplicitly
        {
            add => _viewManager.ViewClosedImplicitly += value;
            remove => _viewManager.ViewClosedImplicitly -= value;
        }
        
        private readonly IViewManager _viewManager;

        public IBindable<string> HighestBusyLayer => _viewManager.HighestBusyLayer;

        public LogViewManagerDecorator(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        public IReadOnlyList<string> GetLayerIds() => _viewManager.GetLayerIds();

        public IViewModel Create(IViewModel parent, string viewName, Transform container, IPayload payload = null)
        {
            Debug.Log("View was created!");
            return _viewManager.Create(parent, viewName, container, payload);
        }

        public IPromise<IViewModel> Open(string viewLayerId, string viewName, IPayload payload = null)
        {
            Debug.Log("View was opened!");
            return _viewManager.Open(viewLayerId, viewName, payload);
        }

        public IPromise OpenExact(string viewLayerId, string viewName, IPayload payload = null)
        {
            Debug.Log("View was opened on exact layer!");
            return _viewManager.OpenExact(viewLayerId, viewName, payload);
        }

        public IPromise CloseExact(string viewLayerId)
        {
            Debug.Log("View was closed on exact layer!");
            return _viewManager.CloseExact(viewLayerId);
        }

        public IPromise Close(string viewLayerId)
        {
            Debug.Log("View was closed!");
            return _viewManager.Close(viewLayerId);
        }

        public string GetViewName(string viewLayerId)
        {
            Debug.Log("View name was got!");
            return _viewManager.GetViewName(viewLayerId);
        }

        public IViewLayer GetLayer(string viewLayerId)
        {
            Debug.Log("Layer was got!");
            return _viewManager.GetLayer(viewLayerId);
        }

        public IViewModel GetView(string viewLayerId)
        {
            Debug.Log("View was got!");
            return _viewManager.GetView(viewLayerId);
        }
    }
}