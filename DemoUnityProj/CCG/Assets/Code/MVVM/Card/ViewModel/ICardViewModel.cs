using UnityEngine;
using UnityMVVM.ViewModelCore;
using UnityMVVM.ViewModelCore.Bindable;

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
        
        IBindable<float> Rotation { get; }
        
        IBindable<Vector2> Position { get; }

        void OnMouseClickDown();

        void OnCardEnterBoard();

        void OnCardExitBoard();

    }
}