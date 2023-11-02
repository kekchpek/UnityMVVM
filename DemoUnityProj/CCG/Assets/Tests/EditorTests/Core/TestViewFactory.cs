using UnityEngine;
using UnityMVVM;
using UnityMVVM.Pool;
using UnityMVVM.ViewModelCore;
using UnityMVVM.ViewModelCore.ViewModelsFactory;

namespace CCG.Tests.Editor.Core
{
    public class TestViewFactory : IViewFactory
    {
        public TView Instantiate<TView>(GameObject viewPrefab, Transform parent, IViewPool viewPool) where TView : IViewInitializer
        {
            return viewPrefab.GetComponent<TView>();
        }

        public void Initialize(IViewInitializer viewInitializer, IViewModel viewModel, bool isPoolableView)
        {
            viewModel.CloseStarted += ViewModelClosing;
            void ViewModelClosing()
            {
                viewModel.CloseStarted -= ViewModelClosing;
                viewModel.Destroy();
            }
        }
    }
}