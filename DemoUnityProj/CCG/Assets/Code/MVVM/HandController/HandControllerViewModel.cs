using CCG.Models.Hand.Service;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.HandController
{
    public class HandControllerViewModel : ViewModel, IHandControllerViewModel
    {
        private readonly IHandService _handService;

        public HandControllerViewModel(IHandService handService)
        {
            _handService = handService;
        }
        
        public void AddRandomCardToHand()
        {
            _handService.AddRandomCardToHand();
        }

        public void SwitchCardsPattern()
        {
            _handService.SwitchCardsPattern();
        }
    }
}