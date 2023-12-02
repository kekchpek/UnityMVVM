using AsyncReactAwait.Bindable;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.TimeCounter
{
    public interface ITimeCounterViewModel : IViewModel
    {
        IBindable<float> TimeInSeconds { get; }
    }
}