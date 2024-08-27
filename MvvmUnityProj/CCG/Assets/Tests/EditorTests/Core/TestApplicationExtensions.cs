using CCG.Core;
using CCG.MVVM.MainMenu;
using CCG.MVVM.PlayButton;

namespace CCG.Tests.Editor.Core
{
    public static class TestApplicationExtensions
    {
        public static void ClickPlayButton(this TestApplication testApp)
        {
            var playButton = testApp
                .GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI)
                .GetSubview<IPlayButtonViewModel>();
            playButton.OnClicked();
        }
    }
}