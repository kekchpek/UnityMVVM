using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager.ViewLayer
{

    /// <summary>
    /// The layer to place a views.
    /// </summary>
    public interface IViewLayer
    {

        /// <summary>
        /// The layer id.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Container for views.
        /// </summary>
        Transform Container { get; }

        /// <summary>
        /// Destroy root view model.
        /// </summary>
        void Clear();

        /// <summary>
        /// Set root view model. Destroy previous if it exists.
        /// </summary>
        /// <param name="viewModel">View model to be set.</param>
        void Set(IViewModel viewModel);

    }
}
