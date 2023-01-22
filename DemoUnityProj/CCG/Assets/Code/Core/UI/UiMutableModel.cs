using UnityEngine;

namespace CCG.Core.UI
{
    public interface IUiMutableModel : IUiModel
    {
        
        Canvas Canvas { get; set; }
        
        void SetCanvasDistance(float canvasDistance);
    }
}