using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.CoolPopup
{
    public interface ICoolPopupViewModel : IViewModel
    {
        
        bool IsClosingAnimationActive { get; }
        
        void OnOpenCoolPopupBtn();
        void OnCloseBtn();
        void SetClosingAnimationActive(bool isActive);
    }
}