using System;
using AsyncReactAwait.Bindable;
using UnityEngine;

namespace CCG.MVVM.Card.Model
{
    public interface ICardModel
    {
        public event Action<ICardModel> Played;
        public event Action<ICardModel> Destroyed;
        
        IBindable<int> Health { get; }
        
        IBindable<int> Attack { get; }
        
        IBindable<int> Mana { get; }
        
        IBindable<string> Description { get; }
        
        IBindable<string> Title { get; }

        IBindable<Texture2D> Icon { get; }
        
        IBindable<int> IndexInHand { get; }

        void Play();

        void Destroy();
    }
}