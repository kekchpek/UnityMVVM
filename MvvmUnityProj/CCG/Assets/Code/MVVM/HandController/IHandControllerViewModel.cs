using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.HandController
{
    public interface IHandControllerViewModel : IViewModel
    {
        void AddRandomCardToHand();
        void SwitchCardsPattern();
    }
}