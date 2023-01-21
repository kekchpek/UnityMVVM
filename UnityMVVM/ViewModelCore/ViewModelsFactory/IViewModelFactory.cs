using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;
using UnityMVVM.ViewManager.ViewLayer;

namespace UnityMVVM.ViewModelCore.ViewModelsFactory
{

    /// <summary>
    /// Factory to create views and view models.
    /// </summary>
    /// <typeparam name="TViewModel">Type of view models, that will be created.</typeparam>
    internal interface IViewModelFactory<out TViewModel> where TViewModel : IViewModel
    {
        /// <summary>
        /// Creates view and its view model
        /// </summary>
        /// <param name="viewLayer">Layer to place a view.</param>
        /// <param name="parent">Parent view model to set to the created view model.</param>
        /// <param name="payload">View model payload.</param>
        /// <returns>Returns created view model to conrol the view.</returns>
        TViewModel Create(IViewLayer viewLayer, 
            [CanBeNull, AllowNull] IViewModel parent,
            [CanBeNull, AllowNull] IPayload payload = null);
    }
}