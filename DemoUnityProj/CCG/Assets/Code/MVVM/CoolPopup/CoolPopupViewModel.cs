using System;
using CCG.Core;
using CCG.MVVM.CoolPopup.Payload;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.CoolPopup
{
    public class CoolPopupViewModel : ViewModel, ICoolPopupViewModel
    {
        private readonly IViewManager _viewManager;
        public bool IsClosingAnimationActive { get; private set; }

        public CoolPopupViewModel(
            ICoolPopupPayload payload,
            IViewManager viewManager)
        {
            if (payload != null && payload.ThrowError)
            {
                throw new Exception("Test exception!!");
            }

            _viewManager = viewManager;
        }

        public void OnOpenCoolPopupBtn()
        {
            OpenView(ViewLayerIds.Popup, ViewNames.CoolPopup, new CoolPopupPayload(false));
        }

        public async void OnOpenCoolPopupWithErrorBtn()
        {
            await OpenView(ViewLayerIds.Popup, ViewNames.CoolPopup, new CoolPopupPayload(true));
        }
        
        public void OnCloseBtn()
        {
            if (_viewManager.GetViewName(ViewLayerIds.Popup) == ViewNames.CoolPopup)
            {
                _viewManager.GetView(ViewLayerIds.Popup)!.Close();
            }
        }

        public void SetClosingAnimationActive(bool isActive)
        {
            IsClosingAnimationActive = isActive;
        }
    }
}