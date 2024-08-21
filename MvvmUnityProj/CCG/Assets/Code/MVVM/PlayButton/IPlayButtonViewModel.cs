using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.PlayButton
{
    public interface IPlayButtonViewModel : IViewModel
    {
        void OnClicked();
    }
}