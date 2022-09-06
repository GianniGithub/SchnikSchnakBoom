using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GellosGames;

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GellosGames
{

    public abstract class EventManager<TAction, TArgs> where TArgs : struct
    {
        protected Dictionary<TAction, UnityEvent<MonoBehaviour, TArgs>> EventDictionary = new Dictionary<TAction, UnityEvent<MonoBehaviour, TArgs>>();
        public void StartListening(TAction eventName, UnityAction<MonoBehaviour, TArgs> listener) 
        {
            if (EventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent<MonoBehaviour, TArgs>();
                thisEvent.AddListener(listener);
                EventDictionary.Add(eventName, thisEvent);
            }
        }
        public void StopListening(TAction eventName, UnityAction<MonoBehaviour, TArgs> listener)
        {
            if (EventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }
        public abstract void TriggerEvent(MonoBehaviour sender, TArgs listener);

    }





}
