using CCG.Core.Camera;
using CCG.MVVM.MainScreen.ViewModel;
using UnityEngine;
using UnityMVVM.ViewManager;
using Zenject;

namespace CCG.Core
{
    public class StartupBehaviour : MonoBehaviour
    {
        private ICameraService _cameraService;
        private IViewManager _viewManager;

        [SerializeField] private UnityEngine.Camera _mainCamera;
        
        [Inject]
        public void Construct(
            ICameraService cameraService,
            IViewManager viewManager)
        {
            _cameraService = cameraService;
            _viewManager = viewManager;
        }
        
        private void Start()
        {
            _cameraService.SetCamera(_mainCamera);
            _viewManager.Open<IMainScreenViewModel>(ViewLayerIds.Main);
        }
    }
}
