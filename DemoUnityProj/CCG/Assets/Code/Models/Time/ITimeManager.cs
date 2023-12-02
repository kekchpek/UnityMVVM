using System;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Promises;

namespace SurvivedWarrior.MVVM.Models.Time
{
    public interface ITimeManager
    {
        IBindable<long> CurrentTimestamp { get; }
        long TimestampSinceStart { get; }
        long LocalTimeOffset { get; }
        void Pause(string timeline);
        void Resume(string timeline);
        CallbackCancelSource AddCallback(long timestamp, Action callback, string timeLine = "Default");
        DateTime TimestampToLocalTime(long timestamp);
        IPromise Await(float seconds);
    }
}