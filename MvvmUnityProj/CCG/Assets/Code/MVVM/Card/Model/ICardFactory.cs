using UnityEngine;

namespace CCG.MVVM.Card.Model
{
    public interface ICardFactory
    {
        ICardMutableModel Create(int health, int attack, int mana,
            string description, string title, Texture2D icon);
    }
}