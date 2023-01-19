using CCG.MVVM.Card.Model;

namespace CCG.MVVM.Card.ViewModel
{
    public class CardPayload : ICardPayload
    {

        public ICardModel Card { get; }
        
        public CardPayload(ICardModel card)
        {
            Card = card;
        }

    }
}