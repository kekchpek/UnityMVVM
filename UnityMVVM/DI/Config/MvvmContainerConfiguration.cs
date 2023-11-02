using UnityMVVM.ViewModelCore.ViewModelsFactory;
// ReSharper disable UnassignedField.Global

namespace UnityMVVM.DI.Config
{
    /// <summary>
    /// The configuration for MVVM container.
    /// </summary>
    public struct MvvmContainerConfiguration
    {
        /// <summary>
        /// The view factory to use for views creation and initialization.
        /// </summary>
        public IViewFactory? ViewFactory;
    }
}