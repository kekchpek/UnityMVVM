using AsyncReactAwait.Promises;
using System;
using UnityMVVM.ViewManager.ViewLayer;

namespace UnityMVVM.ViewModelCore
{

    /// <summary>
    /// Base view model interface.
    /// </summary>
    public interface IViewModel
    {

        /// <summary>
        /// Fired when view model receive start close call.
        /// </summary>
        event Action<IViewModel> CloseStarted;

        /// <summary>
        /// Fired on view model destroyed.
        /// </summary>
        event Action<IViewModel> Destroyed;

        /// <summary>
        /// Layer, on which view was opened.
        /// </summary>
        public IViewLayer Layer { get; }
        
        internal void AddSubview(IViewModel child);
        
        internal void OnOpened();
        
        /// <summary>
        /// Obtains subview of specified type.
        /// If there is no subview of specified type, returns null.
        /// If there are several subviews with this type, returns the first found.
        /// </summary>
        /// <typeparam name="T">The type of a subview.</typeparam>
        /// <returns>Returns subview of specified type or null.</returns>
        T? GetSubview<T>() where T : IViewModel;
        
        /// <summary>
        /// Obtains all subviews of specified type.
        /// If there is no subviews of specified type, returns an empty array.
        /// </summary>
        /// <typeparam name="T">The type of a subviews.</typeparam>
        /// <returns>Returns array of subviews of specified type or an empty array.</returns>
        T[] GetSubviews<T>() where T : IViewModel;

        /// <summary>
        /// Fire <see cref="CloseStarted"/> event and internal view model close handling.
        /// </summary>
        /// <returns>Promise, that indicates close process.</returns>
        IPromise Close();

        /// <summary>
        /// Destroys the view modes and view instantly.
        /// </summary>
        void Destroy();

    }
}