using AsyncReactAwait.Promises;

namespace CCG.Services.Game
{
    public interface IGameService
    {
        IPromise StartGame();
        IPromise OpenMainMenu();
    }
}