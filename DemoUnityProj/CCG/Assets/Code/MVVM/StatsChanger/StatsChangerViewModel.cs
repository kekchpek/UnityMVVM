using CCG.MVVM.Card.Model;
using CCG.MVVM.Hand.Model;
using CCG.MVVM.Hand.Service;
using UnityEngine;
using UnityMVVM.ViewModelCore;
using Random = UnityEngine.Random;

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
    }
}