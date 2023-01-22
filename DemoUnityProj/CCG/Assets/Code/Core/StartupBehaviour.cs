using CCG.Core.Camera;
using CCG.Core.UI;
using CCG.MVVM.MainMenu;
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
        private IUiService _uiService;

        [SerializeField] private UnityEngine.Camera _mainCamera;
        [SerializeField] private Canvas _mainCanvas;
        
        [Inject]
        public void Construct(
            ICameraService cameraService,
            IViewManager viewManager,
            IUiService uiService)
        {
            _cameraService = cameraService;
            _viewManager = viewManager;
            _uiService = uiService;
        }
        
        private void Start()
        {
            _cameraService.SetDefaultCamera(_mainCamera);
            _uiService.SetCanvasDistance(_mainCanvas.planeDistance);
            _uiService.SetCanvas(_mainCanvas);
            _viewManager.Open(ViewLayerIds.Main3d, ViewNames.MainMenu3d);
        }
    }
}
