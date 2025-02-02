using AsyncReactAwait.Bindable;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.SubviewsColorChanger.Color
{
    public interface IColorViewModel : IViewModel
    {
        
        IBindable<UnityEngine.Color> Color { get; }
        
    }
}