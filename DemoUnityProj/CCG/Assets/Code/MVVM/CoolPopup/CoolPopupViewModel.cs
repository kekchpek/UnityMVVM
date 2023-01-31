using System;
using CCG.Core;
using CCG.MVVM.CoolPopup.Payload;
using UnityAuxiliaryTools.Promises;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.CoolPopup
{
    public class CoolPopupViewModel : ViewModel, ICoolPopupViewModel
    {
        public bool IsClosingAnimationActive { get; private set; }

        public CoolPopupViewModel(ICoolPopupPayload payload)
        {
            if (payload != null && payload.ThrowError)
            {
                throw new Exception("Test exception!!");
            }
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