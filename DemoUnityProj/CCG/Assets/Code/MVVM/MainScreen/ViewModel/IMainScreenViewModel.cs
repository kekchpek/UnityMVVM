using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.MainScreen.ViewModel
{
    public interface IMainScreenViewModel : IViewModel
    {
        void OnPopupButtonClicked();
        void OnMainMenuButtonClicked();
        void SetCardsContainer(Transform container);
    }
}