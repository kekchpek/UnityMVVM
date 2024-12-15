using TMPro;
using UnityEngine;
using UnityMVVM;

namespace CCG.MVVM.MainScreen.Subviews.TextView
{
    public class TextView : ViewBehaviour<ITextViewModel>
    {

        [SerializeField]
        protected TMP_Text _text;
        
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SmartBind(ViewModel!.Text, x => _text.text = x);
        }
    }
}