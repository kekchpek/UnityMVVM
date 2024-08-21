using UnityEngine;
using UnityEngine.UI;
using UnityMVVM;

namespace CCG.MVVM.HandController
{
    public class HandControllerView : ViewBehaviour<IHandControllerViewModel>
    {

        [SerializeField] private Button _addCardButton;
        [SerializeField] private Button _changeCardsPatternButton;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            _addCardButton.onClick.AddListener(() => ViewModel.AddRandomCardToHand());
            _changeCardsPatternButton.onClick.AddListener(() => ViewModel.SwitchCardsPattern());
        }

        protected override void OnViewModelClear()
        {
            base.OnViewModelClear();
            _addCardButton.onClick.RemoveAllListeners();
            _changeCardsPatternButton.onClick.RemoveAllListeners();
        }
    }
}