using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager
{

    /// <summary>
    /// Responsible for managing views and view models.
    /// </summary>
    public interface IViewManager
    {

        /// <summary>
        /// Creates viewModel and corresponding view.
        /// </summary>
        /// <typeparam name="T">A view model type.</typeparam>
        /// <param name="parent">Parent view model.</param>
        /// <param name="payload">View model payload.</param>
        /// <returns>Returns created view model.</returns>
        public T Create<T>(IViewModel parent, [AllowNull, CanBeNull] IPayload payload = null) where T : class, IViewModel;

        /// <summary>
        /// Creates view model and corresponding view. Destroys all views on layers above specified.
        /// </summary>
        /// <typeparam name="T">A view model type</typeparam>
        /// <param name="viewLayerId">A layer, where view should be created.</param>
        /// <param name="payload">View model payload.</param>
        public void Open<T>(string viewLayerId, [AllowNull, CanBeNull] IPayload payload = null) where T : class, IViewModel;

        /// <summary>
        /// Destroys all view on specified layer.
        /// </summary>
        /// <param name="viewLayerId">A layer on which views should be destroyed.</param>
        public void Close(string viewLayerId);

    }
}
