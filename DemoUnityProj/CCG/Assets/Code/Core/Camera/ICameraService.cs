namespace CCG.Core.Camera
{
    public interface ICameraService
    {
        void SetDefaultCamera(UnityEngine.Camera camera);
        void UseDefaultCamera();
        void UseCamera(UnityEngine.Camera camera);
    }
}