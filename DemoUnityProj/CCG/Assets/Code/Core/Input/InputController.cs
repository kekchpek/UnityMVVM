using System;
using CCG.Core.Screen;
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

        public Vector2 MousePosition => UnityEngine.Input.mousePosition * _screenScale + _bottomLeftScreenPoint;

        [Inject]
        public void Construct(IScreenAdapter screenAdapter)
        {
            _screenAdapter = screenAdapter;
        }

        public void Update()
        {
            if (Math.Abs(_screenResolution.x - _screenAdapter.ScreenWidth) > Tolerance ||
                Math.Abs(_screenResolution.y - _screenAdapter.ScreenHeight) > Tolerance) {
                
                _screenResolution = new Vector2(_screenAdapter.ScreenWidth, _screenAdapter.ScreenHeight);
                _bottomLeftScreenPoint = _screenAdapter.ScreenPointToWorld(Vector3.zero);
                var topRightScreenPoint = _screenAdapter.ScreenPointToWorld(new Vector3(_screenResolution.x, _screenResolution.y, 0f));
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
