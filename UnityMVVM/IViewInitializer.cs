using UnityMVVM.ViewModelCore;

namespace UnityMVVM
{

    /// <summary>
    /// The interface for initialize the view.
    /// </summary>
    public interface IViewInitializer
    {

        /// <summary>
        /// Sets the view model.
        /// </summary>
        /// <param name="viewModel"></param>
        void SetViewModel(IViewModel viewModel);
    }
}