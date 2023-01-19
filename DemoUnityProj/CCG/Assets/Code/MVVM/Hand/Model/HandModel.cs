using System;
using System.Collections.Generic;
using System.Linq;
using CCG.MVVM.Card.Model;
using UnityEngine;
using UnityMVVM.ViewModelCore.Bindable;

namespace CCG.MVVM.Hand.Model
{
    public class HandModel : IHandMutableModel
    {
        private readonly IList<ICardMutableModel> _cards = new List<ICardMutableModel>();

        private readonly IMutable<bool> _isArcPattern = new Mutable<bool>(true);

        public event Action<ICardModel> CardRemoved;
        public event Action<ICardModel> CardAdded;
        public int MaxCardsCount => Config.ConfigData.MaxCardsInHand;
        
        public IBindable<bool> IsArchPattern => _isArcPattern;

        public void AddCard(ICardMutableModel card)
        {
            if (_cards.Count == MaxCardsCount)
            {
                card.Destroy();
                return;
            }

            _cards.Add(card);
            void OnCardPlayed()
            {
                card.Played -= OnCardPlayed;
                card.Destroyed -= OnCardDestroyed;
                RemoveCard(card);
                card.Destroy();
                Debug.Log("CardPlayed!");
            }
            void OnCardDestroyed()
            {
                card.Played -= OnCardPlayed;
                card.Destroyed -= OnCardDestroyed;
                RemoveCard(card);
            }
            card.Played += OnCardPlayed;
            card.Destroyed += OnCardDestroyed;
            UpdateCardsIndices();
            CardAdded?.Invoke(card);
        }

        private void RemoveCard(ICardMutableModel card)
        {
            _cards.Remove(card);
            CardRemoved?.Invoke(card);
            UpdateCardsIndices();
        }

        private void UpdateCardsIndices()
        {
            var i = 0;
            foreach (var card in _cards)
            {
                card.SetIndexInHand(i++);
            }
        }

        public ICardModel[] GetCards()
        {
            return _cards.Cast<ICardModel>().ToArray();
        }

        public void SwitchCardsPattern()
        {
            _isArcPattern.Value = !_isArcPattern.Value;
        }

        public ICardMutableModel[] GetCardsForMutation()
        {
            return _cards.ToArray();
        }
    }
}