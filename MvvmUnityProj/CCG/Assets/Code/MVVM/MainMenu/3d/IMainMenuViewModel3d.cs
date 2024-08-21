using AsyncReactAwait.Bindable;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.MainMenu
{
    public interface IMainMenuViewModel3d : IViewModel
    {
        IBindable<MainMenuState> State { get; }

        void OnStateChangeCompleted();
    }
}