using CCG.Core;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.CoolPopup
{
    public class CoolPopupViewModel : ViewModel, ICoolPopupViewModel
    {
        public bool IsClosingAnimationActive { get; private set; }

        public void OnOpenCoolPopupBtn()
        {
            OpenView(ViewLayerIds.Popup, ViewNames.CoolPopup);
        }

        public void OnCloseBtn()
        {
            Close();
        }

        public void SetClosingAnimationActive(bool isActive)
        {
            IsClosingAnimationActive = isActive;
        }
    }
}