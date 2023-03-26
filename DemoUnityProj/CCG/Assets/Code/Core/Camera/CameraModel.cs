using AsyncReactAwait.Bindable;
using UnityEngine;
using Zenject;

namespace CCG.Core.Camera
{
    public class CameraModel : ICameraMutableModel, IInitializable
    {
        private readonly IMutable<UnityEngine.Camera> _camera = new Mutable<UnityEngine.Camera>();

        public IBindable<UnityEngine.Camera> CurrenCamera => _camera;
        

        public void Initialize()
        {
            Debug.Log("SECOND INITIALIZED!");
        }
        
        public void SetCamera(UnityEngine.Camera camera)
        {
            _camera.Value = camera;
        }
    }
}