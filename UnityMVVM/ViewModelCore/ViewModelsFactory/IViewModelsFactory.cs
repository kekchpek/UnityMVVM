using UnityEngine;
using UnityMVVM.ViewManager.ViewLayer;

namespace UnityMVVM.ViewModelCore.ViewModelsFactory
{

    /// <summary>
    /// Factory to create view models.
    /// </summary>
    internal interface IViewModelsFactory
    {
        /// <summary>
        /// Creates view and its view model
        /// </summary>
        /// <param name="viewLayer">Layer to place a view.</param>
        /// <param name="parent">Parent view model to set to the created view model.</param>
        /// <param name="parentTransform">The transform to instantiate the view to.</param>
        /// <param name="payload">View model payload.</param>
        /// <returns>Returns created view model to conrol the view.</returns>
        IViewModel Create(IViewLayer viewLayer, 
            IViewModel? parent,
            Transform parentTransform,
            IPayload? payload = null);
    }
}