using System;
using AsyncReactAwait.Promises;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager
{
    /// <summary>
    /// Extensions for view manager.
    /// </summary>
    public static class ViewManagerExtensions
    {
        /// <inheritdoc cref="IViewManager.Create(IViewModel, string, Transform, IPayload)"/>
        /// <typeparam name="T">A view model type.</typeparam>
        public static T Create<T>(this IViewManager viewManager, IViewModel parent, string viewName, Transform container, IPayload? payload = null)
            where T : class, IViewModel
        {
            var viewModel = viewManager.Create(parent, viewName, container, payload);
            if (viewModel is T concreteViewModel)
                return concreteViewModel;
            throw new InvalidCastException($"Can not cast view model of type {viewModel.GetType().Name} to {typeof(T).Name}");
        }

        /// <inheritdoc cref="IViewManager.Open(string, string, IPayload)"/>
        public static async IPromise<T?> Open<T>(this IViewManager viewManager, string viewLayerId, string viewName, IPayload? payload = null)
            where T : class, IViewModel
        {
            var viewModel = await viewManager.Open(viewLayerId, viewName, payload);
            if (viewModel is T concreteViewModel)
                return concreteViewModel;
            throw new InvalidCastException($"Can not cast view model of type {viewModel.GetType().Name} to {typeof(T).Name}");
        }
    }
}