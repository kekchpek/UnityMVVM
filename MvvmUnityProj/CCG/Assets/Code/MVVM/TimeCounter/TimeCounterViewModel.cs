using System;
using AsyncReactAwait.Bindable;
using CCG.Core;
using SurvivedWarrior.MVVM.Models.Time;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;
using Zenject;

namespace CCG.MVVM.TimeCounter
{
    public class TimeCounterViewModel : ViewModel, 
        IInitializable,
        ITimeCounterViewModel
    {
        private readonly IViewManager _viewManager;
        private readonly ITimeManager _timeManager;

        private bool _isCounting;
        
        private readonly IMutable<float> _timeInSeconds = new Mutable<float>();

        public IBindable<float> TimeInSeconds => _timeInSeconds;

        public TimeCounterViewModel(
            IViewManager viewManager,
            ITimeManager timeManager)
        {
            _viewManager = viewManager;
            _timeManager = timeManager;
        }
        
        public void Initialize()
        {
            _viewManager.HighestBusyLayer.Bind(OnHighestLayerChanged);
            _timeManager.CurrentTimestamp.Bind(OnTimeChanged);
        }

        private void OnTimeChanged(long prevVal, long newVal)
        {
            if (_isCounting)
            {
                _timeInSeconds.Value += (newVal - prevVal) / (float)TimeSpan.TicksPerSecond;
            }
        }

        private void OnHighestLayerChanged(string layer)
        {
            _isCounting = layer == ViewLayerIds.MainUI;
        }

        protected override void OnDestroyInternal()
        {
            base.OnDestroyInternal();
            _viewManager.HighestBusyLayer.Unbind(OnHighestLayerChanged);
            _timeManager.CurrentTimestamp.Unbind(OnTimeChanged);
            _isCounting = false;
        }
    }
}