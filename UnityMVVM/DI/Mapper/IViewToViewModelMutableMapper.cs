using System;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.DI.Mapper
{
    
    /// <summary>
    /// An interface to map view types to view models types.
    /// </summary>
    internal interface IViewToViewModelMutableMapper : IViewToViewModelMapper
    {
        void Map<TView, TViewModel>()
            where TViewModel : class, IViewModel
            where TView : IViewBehaviour;

        void Map(Type viewType, Type viewModelType);
    }
}
