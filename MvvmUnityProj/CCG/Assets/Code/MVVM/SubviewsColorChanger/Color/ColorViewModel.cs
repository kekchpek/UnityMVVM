using AsyncReactAwait.Bindable;
using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.SubviewsColorChanger.Color
{
    public class ColorViewModel : ViewModel, IColorViewModel, IColorViewHandle
    {

        private readonly IMutable<UnityEngine.Color> _color = new Mutable<UnityEngine.Color>();

        public IBindable<UnityEngine.Color> Color => _color;
        
        public void SetColor(UnityEngine.Color color)
        {
            _color.Value = color;
        }
        
    }
}