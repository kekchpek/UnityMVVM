using System;

namespace SurvivedWarrior.MVVM.Models.Time
{
    public interface ITimeline
    {
        void AddTime(long ticks);
        CallbackCancelSource AddCallbackIn(long ticksDelay, Action callback);
    }
}