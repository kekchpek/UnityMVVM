using CCG.MVVM.MainScreen.ViewModel;
using UnityEngine;
using UnityMVVM;

namespace CCG.MVVM.MainScreen.View
{
    public class MainScreenView : ViewBehaviour<IMainScreenViewModel>
    {
        [SerializeField] private GameObject _loadingPanel;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            ViewModel.LoadingCompleted += OnLoadingCompleted;
        }

        private void OnLoadingCompleted()
        {
            _loadingPanel.SetActive(false);
        }

        protected override void OnViewModelClear()
        {
            base.OnViewModelClear();
        }
    }
}