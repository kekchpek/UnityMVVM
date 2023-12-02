using AsyncReactAwait.Promises;
using UnityEngine;
using UnityEngine.UI;
using UnityMVVM;

namespace CCG.MVVM.CoolPopup
{
    public class CoolPopupView : ViewBehaviour<ICoolPopupViewModel>
    {
        [SerializeField] private Button _closeBtn;
        [SerializeField] private Button _openPopupButton;
        [SerializeField] private Button _openOtherBth;
        [SerializeField] private Button _openOtherWithErrorBth;
        [SerializeField] private Toggle _animationToggle;

        [SerializeField] private Animator _animator;

        private IControllablePromise _closePromise;
        
        private static readonly int CloseTrigger = Animator.StringToHash("Close");

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            _closeBtn.onClick.AddListener(() => ViewModel!.OnCloseBtn());
            _openOtherBth.onClick.AddListener(() => ViewModel!.OnOpenCoolPopupBtn());
            _openOtherWithErrorBth.onClick.AddListener(() => ViewModel!.OnOpenCoolPopupWithErrorBtn());
            _animationToggle.isOn = ViewModel!.IsClosingAnimationActive;
            _animationToggle.onValueChanged.AddListener(v => ViewModel.SetClosingAnimationActive(v));
        }

        protected override IPromise Close()
        {
            if (!ViewModel!.IsClosingAnimationActive)
            {
                var p = new ControllablePromise();
                p.Success();
                return p;
            }
            if (_closePromise != null)
            {
                return _closePromise;
            }

            _closePromise = new ControllablePromise();
            _animator.SetTrigger(CloseTrigger);
            return _closePromise;
        }

        public void Animation_OnClose()
        {
            _closePromise?.Success();
            _closePromise = null;
        }

        protected override void OnViewModelClear()
        {
            base.OnViewModelClear();
            _closeBtn.onClick.RemoveAllListeners();
            _openOtherBth.onClick.RemoveAllListeners();
            _openOtherWithErrorBth.onClick.RemoveAllListeners();
            _animationToggle.onValueChanged.RemoveAllListeners();
        }
    }
}