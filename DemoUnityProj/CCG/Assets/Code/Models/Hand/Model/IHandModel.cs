using System;
using CCG.MVVM.Card.Model;
using UnityMVVM.ViewModelCore.Bindable;

namespace CCG.Models.Hand.Model
{
    public interface IHandModel
    {

        /// <summary>
        /// Fired on some card was removed from the hand.
        /// </summary>
        event Action<ICardModel> CardRemoved;

        /// <summary>
        /// Fired on some card was added to the hand
        /// </summary>
        event Action<ICardModel> CardAdded;

        /// <summary>
        /// Max cards count in the hand.
        /// </summary>
        int MaxCardsCount { get; }
        
        /// <summary>
        /// Pattern of placing cards in the hand.
        /// </summary>
        IBindable<bool> IsArchPattern { get; }
        
        /// <summary>
        /// Get all cards in hand.
        /// </summary>
        /// <returns>Returns all cards in the hand.</returns>
        ICardModel[] GetCards();
        
        /// <summary>
        /// Changes a representation of cards. Either shows it in a line or in an arc pattern.
        /// </summary>
        void SwitchCardsPattern();
    }
}