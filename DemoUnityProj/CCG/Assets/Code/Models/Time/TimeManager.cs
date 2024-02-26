using System;
using System.Collections.Generic;
using System.Linq;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using AsyncReactAwait.Promises;
using JetBrains.Annotations;
using UnityEngine;

namespace SurvivedWarrior.MVVM.Models.Time
{
    public class TimeManager : MonoBehaviour, ITimeManager
    {

        private long _startupTimestamp;
        private long? _localTimeOffset;

        private readonly IMutable<long> _currentTimestamp = new Mutable<long>();

        private readonly Dictionary<string, ITimeline> _timelines = new();
        private readonly HashSet<string> _pausedTimelines = new();

        private long CurrentTimestampInternal
        {
            get
            {
                if (_startupTimestamp != 0)
                {
                    return _startupTimestamp + TimestampSinceStart;
                }

                return DateTime.UtcNow.Ticks;
            }
        }

        public IBindable<long> CurrentTimestamp
        {
            get
            {
                if (_currentTimestamp.Value == 0)
                {
                    Update();
                }
                return _currentTimestamp;
            }
        }

        public long TimestampSinceStart => (long)(UnityEngine.Time.unscaledTimeAsDouble * TimeSpan.TicksPerSecond);

        public long LocalTimeOffset
        {
            get
            {
                _localTimeOffset ??= (DateTime.Now - DateTime.UtcNow).Ticks;
                return _localTimeOffset.Value;
            }
        }
        
        public void Pause(string timeline)
        {
            _pausedTimelines.Add(timeline);
        }

        public void Resume(string timeline)
        {
            _pausedTimelines.Remove(timeline);
        }

        public CallbackCancelSource AddCallback(long timestamp, [NotNull] Action callback, string timeline = "Default")
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            
            Debug.Log($"Delayed callback submitted: {callback.Method.Name} with time = {new DateTime(timestamp)}");
            
            if (timestamp < CurrentTimestampInternal)
            {
                Debug.Log($"Invoke delayed callback: {callback.Method.Name}");
                callback.Invoke();
                return null;
            }

            if (!_timelines.ContainsKey(timeline))
            {
                _timelines.Add(timeline, new Timeline());
            }
            return _timelines[timeline].AddCallbackIn(timestamp - CurrentTimestampInternal, callback);
        }

        public DateTime TimestampToLocalTime(long timestamp)
        {
            return new DateTime(timestamp, DateTimeKind.Utc).ToLocalTime();
        }

        public IPromise Await(float seconds)
        {
            var promise = new ControllablePromise();
            AddCallback(
                CurrentTimestampInternal + (long)(seconds * TimeSpan.TicksPerSecond),
                () => promise.Success());
            return promise;
        }

        private void Update()
        {
            if (_startupTimestamp == 0)
            {
                _startupTimestamp = DateTime.UtcNow.Ticks - TimestampSinceStart;
                Debug.Log($"Application start time = {new DateTime(_startupTimestamp)}");
            }

            var prevVal = _currentTimestamp.Value;
            var currentTimeStamp = CurrentTimestampInternal;
            _currentTimestamp.Set(currentTimeStamp);
            foreach (var kvp in _timelines
                         .Where(t => !_pausedTimelines.Contains(t.Key)))
            {
                kvp.Value.AddTime(currentTimeStamp - prevVal);
            }
        }
    }
}