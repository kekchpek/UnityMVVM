using TMPro;
using UnityEngine;
using UnityMVVM;

namespace CCG.MVVM.TimeCounter
{
    public class TimeCounterView : ViewBehaviour<ITimeCounterViewModel>
    {
        [SerializeField] private TMP_Text _text;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            ViewModel!.TimeInSeconds.Bind(OnTimeChanged);
        }

        private void OnTimeChanged(float time)
        {
            _text.text = time.ToString("0.0");
        }

        protected override void OnViewModelClear()
        {
            base.OnViewModelClear();
            ViewModel!.TimeInSeconds.Unbind(OnTimeChanged);
        }
    }
}