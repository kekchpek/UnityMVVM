using System;
using CCG.Core.Camera;
using CCG.Core.Screen;
using CCG.Core.UI;
using UnityEngine;
using Zenject;

namespace CCG.Core.Input
{
    public class InputController : MonoBehaviour, IInputController
    {
        private const float Tolerance = 0.01f;
        
        public event Action MouseUp;
        public event Action<Vector2> MousePositionChanged;
        
        private Vector2 _screenResolution;
        private float _screenScale;
        private Vector3 _bottomLeftScreenPoint;

        private IScreenAdapter _screenAdapter;
        private IUiModel _uiModel;
        private ICameraModel _cameraModel;

        private bool? _previousCameraOrthographic;

        public Vector2 MouseWorldPositionOnCanvas => UnityEngine.Input.mousePosition * _screenScale + _bottomLeftScreenPoint;

        [Inject]
        public void Construct(
            ICameraModel cameraModel,
            IScreenAdapter screenAdapter,
            IUiModel uiModel)
        {
            _cameraModel = cameraModel;
            _screenAdapter = screenAdapter;
            _uiModel = uiModel;
        }

        public void Update()
        {
            
            if (Math.Abs(_screenResolution.x - _screenAdapter.ScreenWidth) > Tolerance ||
                Math.Abs(_screenResolution.y - _screenAdapter.ScreenHeight) > Tolerance ||
                _previousCameraOrthographic != _cameraModel.CurrenCamera.Value.orthographic) {
                var canvasDist = _cameraModel.CurrenCamera.Value.orthographic ? 0f : _uiModel.CanvasDistance;
                _screenResolution = new Vector2(_screenAdapter.ScreenWidth, _screenAdapter.ScreenHeight);
                _bottomLeftScreenPoint = _screenAdapter.ScreenPointToWorld(new Vector3(0f, 0f, canvasDist));
                var topRightScreenPoint = _screenAdapter.ScreenPointToWorld(new Vector3(_screenResolution.x, _screenResolution.y, canvasDist));
                _screenScale = (topRightScreenPoint.x - _bottomLeftScreenPoint.x) / _screenResolution.x;
            }
            
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                MouseUp?.Invoke();
            }
            
            MousePositionChanged?.Invoke(UnityEngine.Input.mousePosition * _screenScale + _bottomLeftScreenPoint);
        }
    }
}
