using UnityMVVM.ViewModelCore;
using UnityMVVM.ViewModelCore.Bindable;

namespace CCG.MVVM.MainMenu
{
    public interface IMainMenuViewModel3d : IViewModel
    {
        IBindable<MainMenuState> State { get; }

        void OnStateChangeCompleted();
    }
}