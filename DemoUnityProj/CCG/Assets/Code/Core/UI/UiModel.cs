using UnityEngine;

namespace CCG.Core.UI
{
    public class UiModel : IUiMutableModel
    {
        public float CanvasDistance { get; private set; }

        public Canvas Canvas { get; set; }

        public void SetCanvasDistance(float canvasDistance)
        {
            CanvasDistance = canvasDistance;
        }
    }
}