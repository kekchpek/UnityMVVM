namespace CCG.Core.Camera
{
    public class CameraService : ICameraService
    {
        private readonly ICameraMutableModel _cameraModel;

        public CameraService(ICameraMutableModel cameraModel)
        {
            _cameraModel = cameraModel;
        }
        
        public void SetCamera(UnityEngine.Camera camera)
        {
            _cameraModel.SetCamera(camera);
        }
    }
}