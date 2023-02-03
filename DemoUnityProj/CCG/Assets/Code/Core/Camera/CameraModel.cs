using AsyncReactAwait.Bindable;

namespace CCG.Core.Camera
{
    public class CameraModel : ICameraMutableModel
    {
        private readonly IMutable<UnityEngine.Camera> _camera = new Mutable<UnityEngine.Camera>();

        public IBindable<UnityEngine.Camera> CurrenCamera => _camera;
        
        public void SetCamera(UnityEngine.Camera camera)
        {
            _camera.Value = camera;
        }
    }
}