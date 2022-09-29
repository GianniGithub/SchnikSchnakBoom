using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace GellosGames
{
    public enum NPCActions
    {
        None = 0,
        WeapenSwitch = 201,

        OnSpawn = 301,

        OnKilled = 402,

        OnHit = 501,
        OnDamage = 502,

        OnShoot = 601,

        IsAiming = 701,

    }
    public class NPCEvents : SpownObjects<NPCActions, NPCEventArgs>
    {
        public NPCtype TypeOfNPC { get; }
        public static int NPCCount => NPCcollection.Count;
        static Dictionary<GameObject, NPCEvents> NPCcollection = new Dictionary<GameObject, NPCEvents>();

        private NPCEvents(NPCtype type, GameObject gameObject)
        {
            this.TypeOfNPC = type;
            NPCcollection.Add(gameObject, this);
        }
        public static NPCEvents AddNPC(NPCtype id, GameObject gameObject)
        {
            var thisEventHandler = new NPCEvents(id, gameObject);

            foreach (var col in CollectChilds<NPCEvent>(gameObject))
            {
                col.EventHandler = thisEventHandler;
                col.OnSpawn();
            }
            thisEventHandler.StartListening(NPCActions.OnKilled, (s, e) => RemoveNPC(s.gameObject));

            return thisEventHandler;
        }

        private static void RemoveNPC(GameObject gameObject)
        {
            NPCcollection.Remove(gameObject);
        }

        public override void TriggerEvent(MonoBehaviour sender, NPCEventArgs listener)
        {
            if (EventDictionary.TryGetValue(listener.Action, out var thisEvent))
            {
                thisEvent.Invoke(sender, listener);
            }
        }
    }
    public abstract class NPCEvent : SpownEvent
    {
        public NPCEvents EventHandler;
        public virtual void OnSpawn() { }

    }
    public struct NPCEventArgs
    {
        public readonly NPCActions Action { get; }
        public EventArgs EventInfos { get; }
        public WeaponType Current { get; set; }
        public bool IsAiming { get; }

        public NPCEventArgs(NPCActions action)
        {
            this.Action = action;
            Current = WeaponType.unknown;
            EventInfos = null;
            IsAiming = false;
        }

        public NPCEventArgs(NPCActions action, EventArgs e) : this(action)
        {
            EventInfos = e;
        }
    }
}
