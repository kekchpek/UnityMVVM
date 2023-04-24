using JetBrains.Annotations;
using UnityMVVM.ViewManager.ViewLayer;
using UnityMVVM.ViewManager;

namespace UnityMVVM.ViewModelCore
{

    /// <summary>
    /// Internal interface for initializing view model internal dependencies.
    /// </summary>
    internal interface IViewModelInternal : IViewModel
    {

        void OnOpened();

    }

}
