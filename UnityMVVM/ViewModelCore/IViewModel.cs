using System;
using UnityMVVM.ViewManager.ViewLayer;

namespace UnityMVVM.ViewModelCore
{
    public interface IViewModel
    {

        public event Action OnDestroy;

        public IViewLayer Layer { get; }

        public void Destroy();

    }
}