using CCG.Models.Hand.Model;
using CCG.Models.ImageModel;
using CCG.MVVM.Card.Model;
using UnityEngine;

namespace CCG.Models.Hand.Service
{
    public class HandService : IHandService
    {
        private readonly ICardFactory _cardFactory;
        private readonly IHandMutableModel _handModel;
        private readonly IImageModel _imageModel;

        private int _changingCardIndex;

        public HandService(
            ICardFactory cardFactory,
            IHandMutableModel handModel,
            IImageModel imageModel)
        {
            _cardFactory = cardFactory;
            _handModel = handModel;
            _imageModel = imageModel;
        }
        
        public void Initialize()
        {
            _handModel.CardRemoved += OnCardRemoved;
        }

        public void AddRandomCardToHand()
        {
            var imageIds = _imageModel.GetAllImageIds();
            var randomImageId = imageIds[Random.Range(0, imageIds.Length)];
            _handModel.AddCard(
                _cardFactory.Create(
                    Random.Range(1, 10),
                    Random.Range(0, 100),
                    Random.Range(0, 100),
                    "Random description Random description Random description Random description",
                    "RANDOM CARD",
                    _imageModel.GetImage(randomImageId)));
        }

        public void SwitchCardsPattern()
        {
            _handModel.SwitchCardsPattern();
        }
        

        private void OnCardRemoved(ICardModel card)
        {
            if (_changingCardIndex >= card.IndexInHand.Value)
            {
                _changingCardIndex--;
                _changingCardIndex = Mathf.Max(_changingCardIndex, 0);
            }
        }

        public void ChangeRandomCardStats()
        {
            var cards = _handModel.GetCardsForMutation();
            if (cards.Length == 0)
                return;
            if (cards.Length - 1 < _changingCardIndex)
                _changingCardIndex = 0;
            ChangeRandomStat(cards[_changingCardIndex]);
            _changingCardIndex++;
        }

        public void ClearHand()
        {
            _handModel.RemoveAllCards();
        }

        private void ChangeRandomStat(ICardMutableModel card)
        {
            var statIndex = Random.Range(0, 3);
            var statValue = Random.Range(-2, 10);
            switch (statIndex)
            {
                case 0:
                    card.SetMana(statValue);
                    break;
                case 1:
                    card.SetHealth(statValue);
                    break;
                case 2: 
                    card.SetAttack(statValue);
                    break;
            }
        }
    }
}