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
        event Action CloseStarted;

        /// <summary>
        /// Fired on view model destroyed.
        /// </summary>
        event Action Destroyed;

        /// <summary>
        /// Layer, on which view was opened.
        /// </summary>
        public IViewLayer Layer { get; }

        /// <summary>
        /// Fire <see cref="CloseStarted"/> event and internal view model close handling.
        /// </summary>
        /// <returns>Promise, that indicates close proccess.</returns>
        IPromise Close();

        /// <summary>
        /// Destroys the view modes and view instantly.
        /// </summary>
        void Destroy();

    }
}