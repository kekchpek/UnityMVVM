using CCG.MVVM.MainScreen.ViewModel;
using UnityEngine;
using UnityEngine.UI;
using UnityMVVM;
using UnityMVVM.Pool;

namespace CCG.MVVM.MainScreen.View
{
    public class MainScreenView : ViewBehaviour<IMainScreenViewModel>, IPoolableView
    {
        [SerializeField] private Button _mainMenuButton;
        
        [SerializeField] private Transform _cardsContainer;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            ViewModel!.SetCardsContainer(_cardsContainer);
            _mainMenuButton.onClick.AddListener(() => ViewModel.OnMainMenuButtonClicked());
        }

        protected override void OnViewModelClear()
        {
            base.OnViewModelClear();
            _mainMenuButton.onClick.RemoveAllListeners();
        }

        public void OnTakenFromPool()
        {
            gameObject.SetActive(true);
        }

        public void OnReturnToPool()
        {
            transform.SetParent(null);
            var go = gameObject;
            go.SetActive(false);
            DontDestroyOnLoad(go);
        }
    }
}