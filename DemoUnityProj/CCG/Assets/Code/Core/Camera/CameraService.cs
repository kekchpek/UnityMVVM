using System;

namespace CCG.Core.Camera
{
    public class CameraService : ICameraService
    {
        private readonly ICameraMutableModel _cameraModel;
        
        private UnityEngine.Camera _defaultCamera;

        public CameraService(ICameraMutableModel cameraModel)
        {
            _cameraModel = cameraModel;
        }

        public void SetDefaultCamera(UnityEngine.Camera camera)
        {
            _defaultCamera = camera;
            if (_cameraModel.CurrenCamera.Value == null)
            {
                UseDefaultCamera();
            }
        }

        public void UseDefaultCamera()
        {
            if (_defaultCamera == null)
            {
                throw new InvalidOperationException("Default camera is null");
            }
            UseCamera(_defaultCamera);
        }

        public void UseCamera(UnityEngine.Camera camera)
        {
            if (camera == null)
            {
                throw new ArgumentNullException(nameof(camera));
            }

            if (_cameraModel.CurrenCamera.Value != null)
            {
                _cameraModel.CurrenCamera.Value.gameObject.SetActive(false);
            }
            
            camera.gameObject.SetActive(true);
            _cameraModel.SetCamera(camera);
        }
    }
}