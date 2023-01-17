using System;
using System.Collections.Generic;
using System.Text;

namespace UnityMVVM.ViewModelCore.ViewModelsContainer
{
    internal interface IViewModelsContainer<T> where T : class, IViewModel
    {
        public T? Resolve();
    }
}
