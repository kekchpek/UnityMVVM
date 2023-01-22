using System;
using CCG.Core.Camera;
using CCG.MVVM.MainScreen.ViewModel;
using UnityEngine;
using UnityMVVM;
using Zenject;

namespace CCG.MVVM.MainScreen.View
{
    public class MainScreenView : ViewBehaviour<IMainScreenViewModel>
    {
        [SerializeField] private GameObject _loadingPanel;

        private ICameraService _cameraService;
        
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
            ViewModel.LoadingCompleted += OnLoadingCompleted;
        }

        private void OnLoadingCompleted()
        {
            _loadingPanel.SetActive(false);
        }

        protected override void OnViewModelClear()
        {
            base.OnViewModelClear();
            ViewModel.LoadingCompleted -= OnLoadingCompleted;
        }
    }
}