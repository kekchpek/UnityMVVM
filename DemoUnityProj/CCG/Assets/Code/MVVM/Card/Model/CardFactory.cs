using CCG.MVVM.Hand;
using CCG.MVVM.Hand.Model;
using UnityEngine;

namespace CCG.MVVM.Card.Model
{
    public class CardFactory : ICardFactory
    {
        public ICardMutableModel Create(int health, int attack, int mana, string description, string title, Texture2D icon)
        {
            var card = new CardModel(health, attack, mana, description, title, icon);
            return card;
        }
    }
}