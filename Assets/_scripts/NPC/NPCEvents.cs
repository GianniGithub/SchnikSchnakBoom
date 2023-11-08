using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace GellosGames
{
    public enum NPCEventTrigger
    {
        None = 0,
        WeapenSwitch = 201,

        OnSpawn = 301,

        OnNPCKilled = 402,

        OnHit = 501,
        OnDamage = 502,

        OnShoot = 601,

        IsAiming = 701,

    }
    public class NPCEvents : SpownObjects<NPCEventTrigger, NPCEventArgs>
    {
        public NPCType TypeOfNPC { get; }
        public static int NPCCount => NPCcollection.Count;
        static Dictionary<GameObject, NPCEvents> NPCcollection = new Dictionary<GameObject, NPCEvents>();

        private NPCEvents(NPCType type, GameObject gameObject)
        {
            this.TypeOfNPC = type;
            NPCcollection.Add(gameObject, this);
        }
        public static NPCEvents AddNPC(NPCType id, GameObject gameObject)
        {
            var thisEventHandler = new NPCEvents(id, gameObject);

            foreach (var col in CollectChilds<NPCEvent>(gameObject))
            {
                col.EventHandler = thisEventHandler;
                col.OnNPCSpawn();
            }
            thisEventHandler.StartListening(NPCEventTrigger.OnNPCKilled, (s, e) => RemoveNPC(s.gameObject));

            return thisEventHandler;
        }

        private static void RemoveNPC(GameObject gameObject)
        {
            NPCcollection.Remove(gameObject);
        }

        public override void TriggerEvent(MonoBehaviour sender, NPCEventArgs listener)
        {
            if (EventDictionary.TryGetValue(listener.EventTrigger, out var thisEvent))
            {
                thisEvent.Invoke(sender, listener);
            }
        }
    }
    public abstract class NPCEvent : SpownEvent
    {
        public NPCEvents EventHandler;
        public virtual void OnNPCSpawn() { }

    }
    public struct NPCEventArgs
    {
        public readonly NPCEventTrigger EventTrigger { get; }
        public EventArgs EventInfos { get; }
        public WeaponType Current { get; set; }
        public bool IsAiming { get; }

        public NPCEventArgs(NPCEventTrigger eventTrigger)
        {
            this.EventTrigger = eventTrigger;
            Current = WeaponType.unknown;
            EventInfos = null;
            IsAiming = false;
        }

        public NPCEventArgs(NPCEventTrigger eventTrigger, EventArgs e) : this(eventTrigger)
        {
            EventInfos = e;
        }
    }
}
