using UnityEngine;
using UnityMVVM.Pool;

namespace UnityMVVM.ViewModelCore.ViewModelsFactory
{
    
    /// <summary>
    /// Factory to create and initialize views.
    /// </summary>
    public interface IViewFactory
    {
        /// <summary>
        /// Instantiates a view from a prefab.
        /// </summary>
        /// <param name="viewPrefab">The view prefab.</param>
        /// <param name="parent">Parent to attach view game object to.</param>
        /// <param name="viewPool">The view pool for attempt to reuse old views.</param>
        /// <typeparam name="TView">The type of a view.</typeparam>
        /// <returns>Returns instantiated view.</returns>
        TView Instantiate<TView>(
            GameObject viewPrefab, 
            Transform parent,
            IViewPool? viewPool) 
            where TView : IViewInitializer;

        /// <summary>
        /// Initializing a view.
        /// </summary>
        /// <param name="viewInitializer">The view to initialize.</param>
        /// <param name="viewModel">The view model for the view.</param>
        /// <param name="isPoolableView">Is view used as poolable view or subview of a poolable view.</param>
        void Initialize(IViewInitializer viewInitializer, IViewModel viewModel, bool isPoolableView);
    }
}