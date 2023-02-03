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
        /// Resolve the factory for specified view.
        /// </summary>
        /// <returns>The factory, that creates views.</returns>
        IViewFactory ResolveViewFactory(string viewName);

    }
}
