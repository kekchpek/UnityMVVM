using AsyncReactAwait.Promises;
using CCG.Core;
using CCG.MVVM.MainMenu;
using UnityMVVM.ViewManager;

namespace CCG.Services.Game
{
    public class GameService : IGameService
    {
        
        private readonly IViewManager _viewManager;

        public GameService(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }
        
        public async IPromise StartGame()
        {
            await _viewManager.Open(ViewLayerIds.Main3d, ViewNames.MainScreen3d);
            await _viewManager.Open(ViewLayerIds.MainUI, ViewNames.MainScreen);
        }

        public async IPromise OpenMainMenu()
        {
            var mainMenu3d = await _viewManager.Open(ViewLayerIds.Main3d, ViewNames.MainMenu3d);  
            await _viewManager.Open(ViewLayerIds.MainUI, ViewNames.MainMenuUi, new MainMenuPayloadUi((IMainMenu3dController)mainMenu3d));
        }
    }
}