using UnityMVVM.ViewModelCore;
using UnityMVVM.ViewModelsFactory;

namespace UnityMVVM.DI
{
    public interface IViewsContainer
    {

        IViewModelFactory<T> ResolveFactory<T>() where T : IViewModel;

    }
}
