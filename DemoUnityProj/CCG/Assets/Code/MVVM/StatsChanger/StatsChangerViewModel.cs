using CCG.Models.Hand.Service;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.StatsChanger
{
    public class StatsChangerViewModel : ViewModel, IStatsChangerViewModel
    {
        private readonly IHandService _handService;

        private int _changingCardIndex;

        public StatsChangerViewModel(IHandService handService)
        {
            _handService = handService;
        }

        public void ChangeCardStat()
        {
            _handService.ChangeRandomCardStats();
        }

        protected override void OnDestroyInternal()
        {
            Debug.Log("Stats changer destroyed!");
            base.OnDestroyInternal();
        }
    }
}