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
        /// <param name="parent">Parent view model.</param>
        /// <param name="viewName">The identifier of the view.</param>
        /// <param name="payload">View model payload.</param>
        /// <returns>Returns created view model.</returns>
        public IViewModel Create(IViewModel parent, string viewName, [AllowNull, CanBeNull] IPayload payload = null);

        /// <inheritdoc cref="Create(IViewModel, string, IPayload)"/>
        /// <typeparam name="T">A view model type.</typeparam>
        public T Create<T>(IViewModel parent, string viewName, [AllowNull, CanBeNull] IPayload payload = null) where T : class, IViewModel;


        /// <summary>
        /// Creates view model and corresponding view. Destroys all views on layers above specified.
        /// </summary>
        /// <param name="viewLayerId">A layer, where view should be created.</param>
        /// <param name="payload">View model payload.</param>
        /// <param name="viewName">The identifier of the view.</param>
        public void Open(string viewLayerId, string viewName, [AllowNull, CanBeNull] IPayload payload = null);

        /// <summary>
        /// Destroys all view on specified layer.
        /// </summary>
        /// <param name="viewLayerId">A layer on which views should be destroyed.</param>
        public void Close(string viewLayerId);

    }
}
