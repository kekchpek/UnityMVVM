using CCG.Core.Camera;
using UnityEngine;

namespace CCG.Core.Screen
{
    public class ScreenAdapter : IScreenAdapter
    {


        private const float RatioX = 1600f;
        private const float RatioY = 900f;
        private const float Ratio = RatioX / RatioY;
        
        private readonly ICameraModel _cameraModel;
        public float ScreenWidth => UnityEngine.Screen.width;
        public float ScreenHeight => UnityEngine.Screen.height;
        
        public float ScreenActiveAreaWidthScale => ScreenActiveAreaWidthPx / ScreenWidth;
        public float ScreenActiveAreaHeightScale => ScreenActiveAreaHeightPx / ScreenHeight;
        public float ScreenActiveAreaWidth => RatioX;
        public float ScreenActiveAreaHeight => RatioY;

        public float ScreenActiveAreaWidthPx
        {
            get
            {
                if (ScreenWidth / ScreenHeight > Ratio)
                {
                    return ScreenHeight * Ratio;
                }
                return ScreenWidth;
            }
        }
        public float ScreenActiveAreaHeightPx
        {
            get
            {
                if (ScreenWidth / ScreenHeight < Ratio)
                {
                    return ScreenWidth / Ratio;
                }
                return ScreenHeight;
            }
        }

        public ScreenAdapter(ICameraModel cameraModel)
        {
            _cameraModel = cameraModel;
        }
        
        public Vector3 ScreenPointToWorld(Vector3 screenPoint)
        {
            return _cameraModel.CurrenCamera.Value.ScreenToWorldPoint(screenPoint);
        }

        public Vector3 ScreenActiveAreaToWorld(Vector3 activeAreaPoint)
        {
            activeAreaPoint.y = activeAreaPoint.y / RatioY * ScreenActiveAreaHeightPx;
            activeAreaPoint.x = activeAreaPoint.x / RatioX * ScreenActiveAreaWidthPx;
            if (ScreenWidth / ScreenHeight > Ratio)
            {
                activeAreaPoint.x += (ScreenWidth - ScreenActiveAreaWidthPx) / 2f;
            }
            else
            {
                activeAreaPoint.y += (ScreenHeight - ScreenActiveAreaHeightPx) / 2f;
            }
            return _cameraModel.CurrenCamera.Value.ScreenToWorldPoint(activeAreaPoint);
        }
    }
}