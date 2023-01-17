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

    /// <inheritdoc cref="IViewsContainerAdapter"/>
    public class ViewsContainerAdapter : IViewsContainerAdapter
    {

        private readonly DiContainer _viewsContainer;


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewsContainer">The container to adapt</param>
        public ViewsContainerAdapter(DiContainer viewsContainer)
        {
            _viewsContainer = viewsContainer;
        }

        /// <inheritdoc cref="IViewsContainerAdapter.ResolveFactory{T}"/>
        public IViewModelFactory<T> ResolveFactory<T>() where T : IViewModel
        {
            return _viewsContainer.Resolve<IViewModelFactory<T>>();
        }
    }
}
