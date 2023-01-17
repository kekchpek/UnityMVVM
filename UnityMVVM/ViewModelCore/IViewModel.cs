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
        /// Fired on view model destroyed.
        /// </summary>
        public event Action OnDestroy;

        /// <summary>
        /// Layer, on which view was opened.
        /// </summary>
        public IViewLayer Layer { get; }

        /// <summary>
        /// Destroys the view modes and view.
        /// </summary>
        public void Destroy();

    }
}