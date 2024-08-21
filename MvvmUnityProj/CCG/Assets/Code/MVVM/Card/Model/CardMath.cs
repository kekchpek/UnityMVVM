using CCG.Config;
using UnityEngine;

namespace CCG.MVVM.Card.Model
{
    public static class CardMath
    {
        private const float HndArcMaxAngle = 120;
        private const float ScreenCenter = 0.5f;
        
        public static (Vector2 position, float angle) GetArchTransform(int index, int cardsCount, int maxCards)
        {
            var handArcWidth = 0.1f * maxCards * ConfigData.CardScale;
            var rotationStep = HndArcMaxAngle / maxCards;
            var rotation = -rotationStep * (0.5f * (cardsCount - 1) - index);
            var y = 0.1f * ConfigData.CardScale * Mathf.Cos(rotation * Mathf.PI / 180f);
            var x = ScreenCenter + handArcWidth / 2f * Mathf.Sin(rotation * Mathf.PI / 180f);
            return (new Vector2(x, y), -rotation);
        }

        public static (Vector2 position, float angle) GetRegularTransform(int index, int cardsCount)
        {
            var positionStepX = 0.07f * ConfigData.CardScale;
            var positionX = ScreenCenter - positionStepX * ((cardsCount - 1) / 2f - index);
            return (new Vector2(positionX, 0.1f * ConfigData.CardScale), 0f);
        }
        
    }
}