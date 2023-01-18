using JetBrains.Annotations;
using UnityMVVM.ViewManager.ViewLayer;
using UnityMVVM.ViewManager;

namespace UnityMVVM.ViewModelCore
{

    /// <summary>
    /// Internal interface for initializing view model internal dependencies.
    /// </summary>
    internal interface IViewModelInitializer
    {

        /// <summary>
        /// Sets the internal view model dependencies.
        /// </summary>
        /// <param name="viewManager">View manager.</param>
        /// <param name="layer">Layer on which view was created.</param>
        /// <param name="parent">Parent view.</param>
        void SetInternalDependencies(IViewManager viewManager, IViewLayer layer, [CanBeNull] IViewModel parent);

    }

}
