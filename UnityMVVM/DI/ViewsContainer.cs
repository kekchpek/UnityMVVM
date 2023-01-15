using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityMVVM.ViewModelCore;
using UnityMVVM.ViewModelsFactory;
using Zenject;

namespace UnityMVVM.DI
{
    public class ViewsContainer : IViewsContainer
    {

        private readonly DiContainer _viewsContainer;

        public ViewsContainer(DiContainer viewsContainer)
        {
            _viewsContainer = viewsContainer;
        }

        public IViewModelFactory<T> ResolveFactory<T>() where T : IViewModel
        {
            return _viewsContainer.Resolve<IViewModelFactory<T>>();
        }
    }
}
