using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.MainScreen.ViewModel
{
    public interface IMainScreenViewModel : IViewModel
    {
        void OnMainMenuButtonClicked();
        void SetCardsContainer(Transform container);
    }
}