using CCG.MVVM.Card.Model;

namespace CCG.MVVM.Hand.Model
{
    public interface IHandMutableModel : IHandModel
    {
        /// <summary>
        /// Start tracking card.
        /// </summary>
        /// <param name="card">The card.</param>
        void AddCard(ICardMutableModel card);

        /// <summary>
        /// Gets card by mutable interface.
        /// </summary>
        /// <returns>Returns all cards in hand by mutable interface.</returns>
        ICardMutableModel[] GetCardsForMutation();
    }
}