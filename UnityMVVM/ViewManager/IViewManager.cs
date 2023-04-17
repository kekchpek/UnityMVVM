﻿using AsyncReactAwait.Promises;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager
{

    /// <summary>
    /// Responsible for managing views and view models.
    /// </summary>
    public interface IViewManager
    {

        /// <summary>
        /// Creates viewModel and corresponding view.
        /// </summary>
        /// <param name="parent">Parent view model.</param>
        /// <param name="viewName">The identifier of the view.</param>
        /// <param name="payload">View model payload.</param>
        /// <returns>Returns created view model.</returns>
        public IViewModel Create(IViewModel parent, string viewName, IPayload? payload = null);

        /// <inheritdoc cref="Create(IViewModel, string, IPayload)"/>
        /// <typeparam name="T">A view model type.</typeparam>
        public T Create<T>(IViewModel parent, string viewName, IPayload? payload = null) where T : class, IViewModel;
        
        /// <summary>
        /// Creates view model and corresponding view. Closes all views on layers above specified.
        /// </summary>
        /// <param name="viewLayerId">A layer, where view should be created.</param>
        /// <param name="payload">View model payload.</param>
        /// <param name="viewName">The identifier of the view.</param>
        public IPromise Open(string viewLayerId, string viewName, IPayload? payload = null);

        /// <summary>
        /// Creates view model and corresponding view.
        /// </summary>
        /// <param name="viewLayerId">A layer, where view should be created.</param>
        /// <param name="payload">View model payload.</param>
        /// <param name="viewName">The identifier of the view.</param>
        /// <returns>The promise that indicates open process.</returns>
        public IPromise OpenExact(string viewLayerId, string viewName, IPayload? payload = null);

        /// <summary>
        /// Destroys all view on specified layer.
        /// </summary>
        /// <param name="viewLayerId">A layer on which views should be closed.</param>
        public IPromise Close(string viewLayerId);
        
        /// <summary>
        /// Close all view on specified layer and all layer above(Starting from the top one).
        /// </summary>
        /// <param name="viewLayerId">A layer on which views should be closed.</param>
        public IPromise CloseAbove(string viewLayerId);

        /// <summary>
        /// Gets a name of the view on the specified layer.
        /// </summary>
        /// <param name="viewLayerId">The id of layer to get a view name.</param>
        /// <returns>The view name</returns>
        public string? GetViewName(string viewLayerId);

        /// <summary>
        /// Gets the view model of the view on the specified layer.
        /// </summary>
        /// <param name="viewLayerId">The id of layer to get a view.</param>
        /// <returns>The view model.</returns>
        public IViewModel? GetView(string viewLayerId);
    }
}
