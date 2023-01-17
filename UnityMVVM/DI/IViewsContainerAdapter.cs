using UnityMVVM.ViewModelCore;
using UnityMVVM.ViewModelCore.ViewModelsFactory;

namespace UnityMVVM.DI
{

    /// <summary>
    /// The adapter for views DI container.
    /// </summary>
    public interface IViewsContainerAdapter
    {

        /// <summary>
        /// Resolve the factory for specified view models.
        /// </summary>
        /// <typeparam name="T">Type of view models to be created from factory.</typeparam>
        /// <returns>The factory, that creates views and view models.</returns>
        IViewModelFactory<T> ResolveFactory<T>() where T : IViewModel;

    }
}
