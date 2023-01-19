using CCG.MVVM.Card.Model;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.Card.ViewModel
{
    public interface ICardPayload : IPayload
    {
        ICardModel Card { get; }
    }
}