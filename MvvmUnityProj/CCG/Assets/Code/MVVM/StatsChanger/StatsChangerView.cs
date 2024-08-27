using UnityEngine;
using UnityEngine.UI;
using UnityMVVM;

namespace CCG.MVVM.StatsChanger
{
    public class StatsChangerView : ViewBehaviour<IStatsChangerViewModel>
    {

        [SerializeField] private Button _changeStatsButton;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            _changeStatsButton.onClick.AddListener(() => ViewModel.ChangeCardStat());
        }

        protected override void OnViewModelClear()
        {
            _changeStatsButton.onClick.RemoveAllListeners();
        }
    }
}