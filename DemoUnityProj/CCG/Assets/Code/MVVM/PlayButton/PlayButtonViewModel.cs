using CCG.Core;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.PlayButton
{
    public class PlayButtonViewModel : ViewModel, IPlayButtonViewModel
    {
        private readonly IViewManager _viewManager;

        public PlayButtonViewModel(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }
        
        public async void OnClicked()
        {
            await _viewManager.Close(ViewLayerIds.Main3d);
            await OpenView(ViewLayerIds.MainUI, ViewNames.MainScreen);
        }
    }
}