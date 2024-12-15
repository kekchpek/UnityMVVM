using AsyncReactAwait.Bindable;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.MainScreen.Subviews.TextView
{
    public interface ITextViewModel : IViewModel
    {
        IBindable<string> Text { get; }
    }
}