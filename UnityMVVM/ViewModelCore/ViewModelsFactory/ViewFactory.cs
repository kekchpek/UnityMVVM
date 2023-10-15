using UnityEngine;
using UnityEngine.Scripting;
using UnityMVVM.DI;
using UnityMVVM.Pool;

namespace UnityMVVM.ViewModelCore.ViewModelsFactory
{
    
    /// <summary>
    /// The factory to create and initialize views.
    /// </summary>
    internal class ViewFactory : IViewFactory
    {
        private readonly IViewsContainerAdapter _viewsContainerAdapter;

        /// <summary>
        /// Default constructor to create view factory.
        /// </summary>
        /// <param name="viewsContainerAdapter"></param>
        [Preserve]
        public ViewFactory(IViewsContainerAdapter viewsContainerAdapter)
        {
            _viewsContainerAdapter = viewsContainerAdapter;
        }
        
        /// <inheritdoc/>
        public TView Instantiate<TView>(
            GameObject viewPrefab, 
            Transform transform,
            IViewPool? viewPool) 
            where TView : IViewInitializer
        {
            TView view;
            if (viewPool != null && viewPool.TryPop(out var poolableView))
            {
                view = (TView)poolableView!;
                view.SetParent(transform);
            }
            else
            {
                view = _viewsContainerAdapter.Container.InstantiatePrefabForComponent<TView>(
                    viewPrefab, transform);
                view.SetPool(viewPool);
            }

            return view;
        }

        /// <inheritdoc/>
        public void Initialize(IViewInitializer viewInitializer, IViewModel viewModel, bool isPoolableView)
        {
            viewInitializer.SetViewModel(viewModel);
            viewInitializer.SetPartOfPoolableView(isPoolableView);
        }
    }
}