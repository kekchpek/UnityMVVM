namespace CCG.Core.Camera
{
    public interface ICameraMutableModel : ICameraModel
    {
        void SetCamera(UnityEngine.Camera camera);
    }
}