using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityMVVM.ViewModelCore;
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


        /// <inheritdoc cref="IViewsModelsContainerAdapter.ResolveFactory{T}"/>
        public IViewModelFactory<T> ResolveFactory<T>(string viewName) where T : IViewModel
        {
            return _viewsContainer.ResolveId<IViewModelFactory<T>>(viewName);
        }
    }
}
