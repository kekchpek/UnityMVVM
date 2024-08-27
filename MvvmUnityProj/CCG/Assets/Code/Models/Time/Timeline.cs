using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SurvivedWarrior.MVVM.Models.Time
{
    public class Timeline : ITimeline
    {
        
        private readonly SortedList<long, List<Action>> _callbacks = new();
        private long _ticks;
        
        public void AddTime(long ticks)
        {
            _ticks += ticks;
            if (_callbacks.Any())
            {
                KeyValuePair<long, List<Action>> kvp;
                while (_callbacks.Any() && (kvp = _callbacks.First()).Key < _ticks)
                {
                    var callbacksCollection = new Action[kvp.Value.Count];
                    kvp.Value.CopyTo(callbacksCollection);
                    foreach (var callback in callbacksCollection)
                    {
                        Debug.Log($"Invoke delayed callback: {callback.Method.Name}");
                        callback.Invoke();
                    }

                    _callbacks.Remove(kvp.Key);
                }
            }
        }

        public CallbackCancelSource AddCallbackIn(long ticksDelay, Action callback)
        {
            var timestamp = _ticks + ticksDelay;
            if (!_callbacks.ContainsKey(timestamp))
            {
                _callbacks.Add(timestamp, new List<Action>());
            }
            _callbacks[timestamp].Add(callback);
            return new CallbackCancelSource(() =>
            {
                if (_callbacks.ContainsKey(timestamp))
                    _callbacks[timestamp].Remove(callback);
            });
        }
    }
}