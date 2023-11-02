using System;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Promises;
using CCG.Core.Camera;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.MainMenu
{
    public class MainMenuViewModel3d : ViewModel, IMainMenuViewModel3d, IMainMenu3dController
    {
        private readonly IViewManager _viewManager;
        private readonly ICameraService _cameraService;

        private IControllablePromise _changeStatePromise;

        private readonly IMutable<MainMenuState> _state = new Mutable<MainMenuState>();

        public IBindable<MainMenuState> State => _state;

        public MainMenuViewModel3d(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        public void OnStateChangeCompleted()
        {
            _changeStatePromise?.Success();
            _changeStatePromise = null;
        }
        
        public IPromise SetState(MainMenuState state)
        {
            _changeStatePromise?.Fail(new Exception("Other state changing triggered."));
            _changeStatePromise = new ControllablePromise();
            _state.Value = state;
            return _changeStatePromise;
        }
    }
}