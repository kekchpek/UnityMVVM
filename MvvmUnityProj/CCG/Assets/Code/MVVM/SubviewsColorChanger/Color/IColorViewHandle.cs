using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.SubviewsColorChanger.Color
{
    public interface IColorViewHandle : IViewModel
    {
        void SetColor(UnityEngine.Color color);
    }
}