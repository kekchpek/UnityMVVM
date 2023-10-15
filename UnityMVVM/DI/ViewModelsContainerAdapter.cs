using UnityMVVM.ViewModelCore.ViewModelsFactory;
using Zenject;

namespace UnityMVVM.DI
{

    /// <inheritdoc cref="IViewsModelsContainerAdapter"/>
    internal class ViewModelsContainerAdapter : IViewsModelsContainerAdapter
    {

        private readonly DiContainer _viewsContainer;


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewsContainer">The container to adapt</param>
        public ViewModelsContainerAdapter(DiContainer viewsContainer)
        {
            _viewsContainer = viewsContainer;
        }

        /// <summary>
        /// Di container for view-model layer.
        /// </summary>
        public DiContainer Container => _viewsContainer;


        /// <inheritdoc cref="IViewsModelsContainerAdapter.ResolveViewFactory"/>
        public IViewModelsFactory ResolveViewFactory(string viewName)
        {
            return _viewsContainer.ResolveId<IViewModelsFactory>(viewName);
        }
    }
}
