using CCG.Core;
using CCG.Services.Game;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.PlayButton
{
    public class PlayButtonViewModel : ViewModel, IPlayButtonViewModel
    {
        private readonly IGameService _gameService;

        public PlayButtonViewModel(IGameService gameService)
        {
            _gameService = gameService;
        }
        
        public async void OnClicked()
        {
            await _gameService.StartGame();
        }
    }
}