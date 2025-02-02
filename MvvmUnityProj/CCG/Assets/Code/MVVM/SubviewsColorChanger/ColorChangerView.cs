using UnityEngine;
using UnityEngine.UI;
using UnityMVVM;

namespace CCG.MVVM.SubviewsColorChanger
{
    public class ColorChangerView : ViewBehaviour<IColorChangerViewModel>
    {

        [SerializeField]
        private Button _changeColorButton;
        
        [SerializeField]
        private Button _closeButton;

        protected override void Awake()
        {
            base.Awake();
            _changeColorButton.onClick.AddListener(() => ViewModel?.ChangeColor());
            _closeButton.onClick.AddListener(() => ViewModel?.Close());
        }
        
        

    }
}