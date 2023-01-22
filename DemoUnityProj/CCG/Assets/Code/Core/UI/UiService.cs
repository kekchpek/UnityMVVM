using CCG.Core.Camera;
using UnityEngine;

namespace CCG.Core.UI
{
    public class UiService : IUiService
    {
        private readonly IUiMutableModel _uiMutableModel;
        private readonly ICameraModel _cameraModel;

        public UiService(IUiMutableModel uiMutableModel,
             ICameraModel cameraModel)
        {
            _uiMutableModel = uiMutableModel;
            _cameraModel = cameraModel;
            _cameraModel.CurrenCamera.Bind(OnCameraChanged);
        }

        private void OnCameraChanged(UnityEngine.Camera camera)
        {
            if (_uiMutableModel.Canvas != null)
            {
                _uiMutableModel.Canvas.worldCamera = camera;
            }
        }

        public void SetCanvas(Canvas canvas)
        {
            _uiMutableModel.Canvas = canvas;
            _uiMutableModel.Canvas.planeDistance = _uiMutableModel.CanvasDistance;
            _uiMutableModel.Canvas.worldCamera = _cameraModel.CurrenCamera.Value;
        }
        
        public void SetCanvasDistance(float canvasDistance)
        {
            _uiMutableModel.SetCanvasDistance(canvasDistance);
            if (_uiMutableModel.Canvas != null)
            {
                _uiMutableModel.Canvas.planeDistance = canvasDistance;
            }
        }
    }
}