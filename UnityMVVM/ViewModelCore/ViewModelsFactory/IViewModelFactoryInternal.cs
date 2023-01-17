using System;


namespace UnityMVVM.ViewModelCore.ViewModelsFactory
{

    /// <summary>
    /// Internal interface for view model factory.
    /// </summary>
    /// <typeparam name="TViewModel">The type of view model to set to the view.</typeparam>
    internal interface IViewModelFactoryInternal<TViewModel> : IViewModelFactory<TViewModel> where TViewModel : IViewModel
    {

        /// <summary>
        /// Firede on some view model created.
        /// </summary>
        event Action<TViewModel> ViewModelCreated;
    }

}
