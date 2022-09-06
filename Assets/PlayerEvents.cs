using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GellosGames.PlayerEvents;

namespace GellosGames
{
    public enum PlayerID
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        all = 100,
        me = 101,
        testPlayer = 102
    }
    public enum PlayerActions
    {
        None = 0,
        UserSignedIn = 101,
        UserLoggedOut = 102,

        WeapenSwitch = 201,

        OnSpawn = 301,

        OnKilled = 402,

        OnHit = 501,
        OnDamage = 502,

        OnShoot = 601,

        IsAiming = 701,

        PlayerControllerEventsRegisterd = 7801,
    }
    public class PlayerEvents : EventManager<PlayerActions, PlayerEventArgs>
    {
        public PlayerID id { get; }
        public int PlayerSlot => (int)id;
        static Dictionary<GameObject, PlayerID> playerDict = new Dictionary<GameObject, PlayerID>();
        static PlayerEvents[] allPlayerEvents = new PlayerEvents[4];

        public PlayerEvents(PlayerID playerID, GameObject gameObject)
        {
            id = playerID;
        }

        public static PlayerEvents AddPlayer(PlayerID playerID, GameObject gameObject)
        {
            var thisEventHandler = new PlayerEvents(playerID, gameObject);

            allPlayerEvents[(int)playerID] = thisEventHandler;
            playerDict.Add(gameObject, playerID);

            List<PlayerEvent> playerEventComponents = new List<PlayerEvent>();
            gameObject.GetComponentsInChildren(true, playerEventComponents);

            foreach (var col in playerEventComponents)
            {
                col.EventHandler = thisEventHandler;
                col.OnSpawn();
            }

            thisEventHandler.StartListening(PlayerActions.OnKilled, (s, e) => RemovePlayer(e.From, s.gameObject));

            return thisEventHandler;
        }
        public static void RemovePlayer(PlayerID playerID, GameObject gameObject)
        {
            allPlayerEvents[((int)playerID)] = null;
            playerDict.Remove(gameObject);
        }
        public static PlayerEvents GetPlayerEventsHandler(PlayerID playerID)
        {
            int id = (int)playerID;
            //if (allPlayerEvents[id] == null) Debug.LogError(playerID.ToString() + ": Player does not Exist anymore");
            return allPlayerEvents[id];
        }
        public static PlayerEvents GetPlayerEventsHandler(GameObject player)
        {
            int id = (int)playerDict[player];
            return allPlayerEvents[id];
        }

        public override void TriggerEvent(MonoBehaviour sender, PlayerEventArgs listener)
        {
            if (EventDictionary.TryGetValue(listener.Action, out var thisEvent))
            {
                listener.From = id;
                thisEvent.Invoke(sender, listener);
            }
        }
    }
    public abstract class PlayerEvent : MonoBehaviour
    {
        public PlayerEvents EventHandler;
        public virtual void OnSpawn() { }

    }
    public struct PlayerEventArgs
    {
        public readonly PlayerActions Action { get; }
        public EventArgs EventInfos { get; }
        public PlayerID From { get; set; }
        public Weapen Current { get; set; }
        public bool IsAiming { get; }

        public PlayerEventArgs(PlayerActions action)
        {
            this.Action = action;
            From = PlayerID.me;
            Current = Weapen.unknown;
            EventInfos = null;
            IsAiming = false;
        }

        public PlayerEventArgs(PlayerActions action, EventArgs e) : this(action)
        {
            EventInfos = e;
        }
        public PlayerEventArgs(PlayerActions action, bool isAiming) : this(action)
        {
            IsAiming = isAiming;
        }
        public PlayerEventArgs(PlayerActions action, bool isAiming, Weapen current) : this(action)
        {
            IsAiming = isAiming;
            Current = current;
        }
    }
}