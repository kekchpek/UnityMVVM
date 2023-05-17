using UnityEngine;
using UnityEngine.UI;
using UnityMVVM;

namespace CCG.MVVM.PlayButton
{
    public class PlayButtonView : ViewBehaviour<IPlayButtonViewModel>
    {
        [SerializeField] private Button _button;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            _button.onClick.AddListener(ViewModel!.OnClicked);
        }

        protected override void OnViewModelClear()
        {
            _button.onClick.RemoveListener(ViewModel!.OnClicked);
            base.OnViewModelClear();
        }
        
    }
}