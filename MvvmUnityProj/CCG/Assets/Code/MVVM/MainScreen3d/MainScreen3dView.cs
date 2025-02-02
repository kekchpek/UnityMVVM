using CCG.Core.Camera;
using UnityEngine;
using UnityMVVM;
using UnityMVVM.ViewModelCore;
using Zenject;

namespace CCG.MVVM.MainScreen3d
{
    public class MainScreen3dView : ViewBehaviour<IViewModel>
    {
        [SerializeField] private Camera _camera;

        private ICameraService _cameraService;

        [Inject]
        public void Construct(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        protected override void Awake()
        {
            base.Awake();
            _cameraService.UseCamera(_camera);
        }
    }
}