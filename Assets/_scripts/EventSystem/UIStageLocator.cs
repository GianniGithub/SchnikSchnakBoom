using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GellosGames
{

    /// <summary>
    /// Model View Model 
    /// </summary>
    public delegate void StageDelegate<T>(MonoBehaviour sender, Event<T> e) where T : Enum;

    public abstract class UIStageLocator<TEvent> where TEvent : Enum
    {
        /// <summary>
        /// For specific Single Event Listener which do not need a call on each event ( Filtered ) 
        /// </summary>
        private readonly Dictionary<TEvent, List<StageDelegate<TEvent>>> singleEventChangedListener = new Dictionary<TEvent, List<StageDelegate<TEvent>>>();

        public abstract void RunEvent(MonoBehaviour sender, Event<TEvent> e);

        protected virtual void RunSpecificEvent(MonoBehaviour sender, Event<TEvent> e)
        {
            // Invoke after stage changed event listener
            if (singleEventChangedListener.TryGetValue(e.StageNow, out List<StageDelegate<TEvent>> listeners))
                foreach (var listener in listeners)
                    listener(sender, e);
        }

        public  void AddActionListener(StageDelegate<TEvent> listener, params TEvent[] filter)
        {
            foreach (TEvent stage in filter)
            {
                if (singleEventChangedListener.TryGetValue(stage, out List<StageDelegate<TEvent>> listeners))
                    listeners.Add(listener);
                else
                    singleEventChangedListener.Add(stage, new List<StageDelegate<TEvent>>() { listener });
            }
        }
        public  void RemoveActionListener(StageDelegate<TEvent> listener, params TEvent[] filter)
        {
            try
            {
                foreach (TEvent stage in filter)
                {
                    singleEventChangedListener[stage].Remove(listener);
                }
            }
            catch (Exception)
            {
                // Unload Error will ignored because of unload Errors of Unity
            }
        }

    }

    /// <summary>
    /// Like EventSystem event args
    /// </summary>
    public class Event<TEvent> : EventArgs where TEvent : Enum
    {
        public readonly TEvent StageNow;

        public Event() { }
        public Event(TEvent targetStage)
        {
            this.StageNow = targetStage;
        }
    }
}