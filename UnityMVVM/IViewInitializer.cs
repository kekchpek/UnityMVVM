using UnityMVVM.ViewModelCore;

namespace UnityMVVM
{

    /// <summary>
    /// The interface for initialize the view.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewInitializer<in T> where T : IViewModel
    {

        /// <summary>
        /// Sets the view model.
        /// </summary>
        /// <param name="viewModel"></param>
        void SetViewModel(T viewModel);
    }
}