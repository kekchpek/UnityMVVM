using System;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.DI.Mapper
{
    internal interface IViewToViewModelMutableMapper : IViewToViewModelMapper
    {
        void Map<TView, TViewModel>()
            where TViewModel : class, IViewModel
            where TView : IViewBehaviour;

        void Map(Type viewType, Type viewModelType);
    }
}
