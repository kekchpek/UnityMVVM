using System;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.MainScreen.ViewModel
{
    public interface IMainScreenViewModel : IViewModel
    {

        event Action LoadingCompleted;

    }
}