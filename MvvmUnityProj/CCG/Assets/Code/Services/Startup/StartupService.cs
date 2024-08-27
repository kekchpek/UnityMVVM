using AsyncReactAwait.Promises;
using CCG.Core;
using CCG.MVVM.MainMenu;
using CCG.Services.Game;
using UnityMVVM.ViewManager;

namespace CCG.Services.Startup
{
    public class StartupService : IStartupService
    {
        private readonly IGameService _gameService;

        public StartupService(
            IGameService gameService)
        {
            _gameService = gameService;
        }
        
        public async IPromise Startup()
        {
            await _gameService.OpenMainMenu();
        }
    }
}