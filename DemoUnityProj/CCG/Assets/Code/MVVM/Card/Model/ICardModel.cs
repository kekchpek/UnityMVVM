using System;
using AsyncReactAwait.Bindable;
using UnityEngine;

namespace CCG.MVVM.Card.Model
{
    public interface ICardModel
    {
        IBindable<int> Health { get; }
        
        IBindable<int> Attack { get; }
        
        IBindable<int> Mana { get; }
        
        IBindable<string> Description { get; }
        
        IBindable<string> Title { get; }

        IBindable<Texture2D> Icon { get; }
        
        IBindable<int> IndexInHand { get; }
        
        public event Action Played;
        public event Action Destroyed;

        void Play();

        void Destroy();
    }
}