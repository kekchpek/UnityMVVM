using UnityEngine;

namespace CCG.Core.Screen
{
    public interface IScreenAdapter
    {
        float ScreenWidth { get; }
        float ScreenHeight { get; }
        float ScreenActiveAreaWidthScale { get; }
        float ScreenActiveAreaHeightScale { get; }
        float ScreenActiveAreaWidth { get; }
        float ScreenActiveAreaHeight { get; }
        float ScreenActiveAreaWidthPx { get; }
        float ScreenActiveAreaHeightPx { get; }
        Vector3 ScreenPointToWorld(Vector3 screenPoint);
        Vector3 ScreenActiveAreaToWorld(Vector3 activeAreaPoint);
    }
}