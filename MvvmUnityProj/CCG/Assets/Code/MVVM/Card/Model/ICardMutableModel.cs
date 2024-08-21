using UnityEngine;

namespace CCG.MVVM.Card.Model
{
    public interface ICardMutableModel : ICardModel
    {
        void SetHealth(int health);
        void SetAttack(int attack);
        void SetMana(int mana);
        void SetDescription(string desc);
        void SetTitle(string title);
        void SetIcon(Texture2D icon);
        void SetIndexInHand(int i);
    }
}