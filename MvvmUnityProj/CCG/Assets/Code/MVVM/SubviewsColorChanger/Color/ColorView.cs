using UnityEngine;
using UnityEngine.UI;
using UnityMVVM;

namespace CCG.MVVM.SubviewsColorChanger.Color
{
    public class ColorView : ViewBehaviour<IColorViewModel>
    {

        [SerializeField]
        private Graphic _graphic;
        
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SmartBind(ViewModel!.Color, x => _graphic.color = x);
        }
    }
}