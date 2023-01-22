using CCG.Core.Camera;
using CCG.MVVM.MainScreen.ViewModel;
using UnityEngine;
using UnityEngine.UI;
using UnityMVVM;
using Zenject;

namespace CCG.MVVM.MainScreen.View
{
    public class MainScreenView : ViewBehaviour<IMainScreenViewModel>
    {
        private ICameraService _cameraService;

        [SerializeField] private Button _mainMenuButton;
        
        [Inject]
        public void Construct(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        private void Awake()
        {
            _cameraService.UseDefaultCamera();
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            _mainMenuButton.onClick.AddListener(() => ViewModel.OnMainMenuButtonClicked());
        }

        protected override void OnViewModelClear()
        {
            base.OnViewModelClear();
            _mainMenuButton.onClick.RemoveAllListeners();
        }
    }
}