using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewModelsFactory
{
    public interface IViewModelFactory<out TViewModel> where TViewModel : IViewModel
    {
        TViewModel Create(Transform viewContainer, IViewModel parent);
    }
}