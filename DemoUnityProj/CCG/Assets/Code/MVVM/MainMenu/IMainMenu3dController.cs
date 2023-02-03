using AsyncReactAwait.Promises;

namespace CCG.MVVM.MainMenu
{
    public interface IMainMenu3dController
    {
        IPromise SetState(MainMenuState state);
    }
}