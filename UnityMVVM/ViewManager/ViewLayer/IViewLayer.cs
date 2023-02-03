using AsyncReactAwait.Promises;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager.ViewLayer
{

    /// <summary>
    /// The layer to place a views.
    /// </summary>
    public interface IViewLayer
    {

        /// <summary>
        /// The layer id.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Container for views.
        /// </summary>
        Transform Container { get; }

        /// <summary>
        /// Close root view model.
        /// </summary>
        /// <returns>A promise that indicates the close proccess.</returns>
        IPromise Clear();

        /// <summary>
        /// Destroy root view model.
        /// </summary>
        void ClearInstantly();

        /// <summary>
        /// Set root view model. Destroy previous if it exists.
        /// </summary>
        /// <param name="viewModel">View model to be set.</param>
        void Set(IViewModel viewModel);

    }
}
