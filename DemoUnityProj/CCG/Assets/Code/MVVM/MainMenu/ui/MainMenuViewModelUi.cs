using AsyncReactAwait.Bindable;
using CCG.Core;
using CCG.MVVM.CoolPopup.Payload;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.MainMenu
{
    public class MainMenuViewModelUi : ViewModel, IMainMenuViewModelUi
    {
        private readonly IViewManager _viewManager;

        private readonly IMainMenu3dController _mainMenu3dController;

        private readonly IMutable<bool> _isInteractable = new Mutable<bool>(true);
        private readonly IMutable<bool> _playButtonShown = new Mutable<bool>(true);
        private readonly IMutable<bool> _backButtonShown = new Mutable<bool>();
        private readonly IMutable<bool> _statesButtonsShown = new Mutable<bool>(true);

        public IBindable<bool> IsInteractable => _isInteractable;
        public IBindable<bool> PlayButtonShown => _playButtonShown;
        public IBindable<bool> BackButtonShown => _backButtonShown;
        public IBindable<bool> StatesButtonsShown => _statesButtonsShown;

        public MainMenuViewModelUi(
            IViewManager viewManager,
            IMainMenuPayloadUi payload)
        {
            _viewManager = viewManager;
            _mainMenu3dController = payload.Controller;
        }

        public void OnSwitchStateButtonPressed(MainMenuState state)
        {
            _isInteractable.Value = false;
            _mainMenu3dController.SetState(state).OnSuccess(() =>
            {
                _backButtonShown.Value = state != MainMenuState.None;
                _statesButtonsShown.Value = state == MainMenuState.None;
                _playButtonShown.Value = state == MainMenuState.None;
                
                _isInteractable.Value = true;
            });
        }

        public async void OnPlayButtonPressed()
        {
            await _viewManager.CloseAbove(ViewLayerIds.Main3d);
            await _viewManager.Open(ViewLayerIds.MainUI, ViewNames.MainScreen);
        }

        public void OnCoolPopupBtn()
        {
            OpenView(ViewLayerIds.Popup, ViewNames.CoolPopup, new CoolPopupPayload(false));
        }
    }
}