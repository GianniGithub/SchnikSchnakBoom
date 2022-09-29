using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GellosGames
{
    public abstract class SpownObjects<TAction, TArgs> : EventManager<TAction, TArgs> where TArgs : struct
    {
        protected static List<T> CollectChilds<T>(GameObject gameObject) where T : SpownEvent
        {
            List<T> EventComponents = new List<T>();
            gameObject.GetComponentsInChildren(true, EventComponents);
            return EventComponents;
        }
    }
    public abstract class SpownEvent : MonoBehaviour
    {

    }
}
