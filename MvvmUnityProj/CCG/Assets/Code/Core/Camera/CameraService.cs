namespace CCG.Core.Camera
{
    public class CameraService : ICameraService
    {
        private readonly ICameraMutableModel _cameraMutableModel;

        public CameraService(ICameraMutableModel cameraMutableModel)
        {
            _cameraMutableModel = cameraMutableModel;
        }
        
        public void UseCamera(UnityEngine.Camera camera)
        {
            _cameraMutableModel.SetCamera(camera);
        }
    }
}