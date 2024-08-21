using AsyncReactAwait.Bindable;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.Card.ViewModel
{
    public interface ICardViewModel : IViewModel
    {
        IBindable<int> Health { get; }
        
        IBindable<int> Attack { get; }
        
        IBindable<int> Mana { get; }
        
        IBindable<string> Description { get; }
        
        IBindable<string> Title { get; }

        IBindable<Texture2D> Icon { get; }
        
        IBindable<bool> IsSelected { get; }
        
        IBindable<bool> IsOverBoard { get; }
        
        IBindable<float> RotationInHand { get; }
        
        IBindable<Vector2> PositionInHand { get; }

        void OnMouseClickDown();
        void OnMouseClickUp();

        void OnCardEnterBoard();

        void OnCardExitBoard();

    }
}