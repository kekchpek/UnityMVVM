using System;

namespace UnityMVVM.DI.Mapper
{
    internal interface IViewToViewModelMapper
    {

        Type GetViewModelForView(Type viewType);

    }
}
