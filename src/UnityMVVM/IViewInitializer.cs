using UnityEngine;
using UnityMVVM.Pool;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM
{

    /// <summary>
    /// The interface for initialize the view.
    /// </summary>
    public interface IViewInitializer
    {

        /// <summary>
        /// Sets the view model.
        /// </summary>
        /// <param name="viewModel"></param>
        void SetViewModel(IViewModel viewModel);

        /// <summary>
        /// Sets the parent of view game object.
        /// </summary>
        /// <param name="parent"></param>
        void SetParent(Transform parent);

        /// <summary>
        /// Mark view as part of pooled view.
        /// </summary>
        /// <param name="isPartOfPooledView"></param>
        void SetPartOfPoolableView(bool isPartOfPooledView);

        /// <summary>
        /// Sets the view pool.
        /// </summary>
        /// <param name="viewPool"></param>
        void SetPool(IViewPool? viewPool);
    }
}