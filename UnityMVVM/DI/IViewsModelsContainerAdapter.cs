using UnityMVVM.ViewModelCore;
using UnityMVVM.ViewModelCore.ViewModelsFactory;
using Zenject;

namespace UnityMVVM.DI
{

    /// <summary>
    /// The adapter for view models DI container.
    /// </summary>
    internal interface IViewsModelsContainerAdapter
    {

        DiContainer Container { get; }


        /// <summary>
        /// Resolve the factory for specified view models.
        /// </summary>
        /// <typeparam name="T">Type of view models to be created from factory.</typeparam>
        /// <returns>The factory, that creates views and view models.</returns>
        IViewModelFactory<T> ResolveFactory<T>() where T : IViewModel;

    }
}
