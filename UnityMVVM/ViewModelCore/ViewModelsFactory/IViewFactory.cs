using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;
using UnityMVVM.ViewManager.ViewLayer;

namespace UnityMVVM.ViewModelCore.ViewModelsFactory
{

    /// <summary>
    /// Factory to create views.
    /// </summary>
    internal interface IViewFactory
    {
        /// <summary>
        /// Creates view and its view model
        /// </summary>
        /// <param name="viewLayer">Layer to place a view.</param>
        /// <param name="parent">Parent view model to set to the created view model.</param>
        /// <param name="payload">View model payload.</param>
        /// <returns>Returns created view model to conrol the view.</returns>
        IViewModelInternal Create(IViewLayer viewLayer, 
            IViewModel? parent,
            IPayload? payload = null);
    }
}