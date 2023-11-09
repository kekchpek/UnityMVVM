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
        public bool IsClosingAnimationActive { get; private set; } = true;

        public CoolPopupViewModel(
            ICoolPopupPayload payload,
            IViewManager viewManager)
        {
            if (payload is { ThrowError: true })
            {
                throw new CoolPopupException("Test exception!!");
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
            Close();
        }

        public void SetClosingAnimationActive(bool isActive)
        {
            IsClosingAnimationActive = isActive;
        }
    }
}