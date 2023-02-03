using AsyncReactAwait.Bindable;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.MainMenu
{
    public interface IMainMenuViewModelUi : IViewModel
    {
        IBindable<bool> IsInteractable { get; }
        IBindable<bool> PlayButtonShown { get; }
        IBindable<bool> BackButtonShown { get; }
        IBindable<bool> StatesButtonsShown { get; }
        void OnSwitchStateButtonPressed(MainMenuState state);
        void OnPlayButtonPressed();
        void OnCoolPopupBtn();
    }
}