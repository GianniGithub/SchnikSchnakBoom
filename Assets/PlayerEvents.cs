using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        OnAdded = 302,

        OnKilled = 402,

        OnHit = 501,
        OnDamage = 502,

        OnShoot = 601,

        PlayerControllerEventsRegisterd = 701,
    }
    public class PlayerEvents
    {
        public static PlayerEvents Global; // Init on SpawnPlayer
        public PlayerID id { get; }
        public int PlayerSlot => (int)id;
        static Dictionary<GameObject, PlayerID> playerDict = new Dictionary<GameObject, PlayerID>();
        static PlayerEvents[] allPlayerEvents = new PlayerEvents[4];

        Dictionary<PlayerActions, UnityEvent<MonoBehaviour, PlayerEventInfos>> playerEventDictionary = new Dictionary<PlayerActions, UnityEvent<MonoBehaviour, PlayerEventInfos>>();
        GameObject player;

        public PlayerEvents(PlayerID playerID, GameObject gameObject)
        {
            id = playerID;
            player = gameObject;
        }

        public static void AddPlayer(PlayerID playerID, GameObject gameObject)
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

            Global.TriggerEvent(null, new PlayerEventInfos(PlayerActions.OnAdded) { From = playerID });
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
        public void StartListening(PlayerActions eventName, UnityAction<MonoBehaviour, PlayerEventInfos> listener)
        {
            if(playerEventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent<MonoBehaviour, PlayerEventInfos>();
                thisEvent.AddListener(listener);
                playerEventDictionary.Add(eventName, thisEvent);
            }
        }
        public void StopListening(PlayerActions eventName, UnityAction<MonoBehaviour, PlayerEventInfos> listener)
        {
            if(playerEventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }
        public void TriggerEvent(MonoBehaviour sender, PlayerEventInfos listener)
        {
            if (playerEventDictionary.TryGetValue(listener.action, out var thisEvent))
            {
                listener.From = id;
                thisEvent?.Invoke(sender, listener);
            }
        }
    }
    public abstract class PlayerEvent : MonoBehaviour
    {
        public PlayerEvents EventHandler;
        public virtual void OnSpawn() { }
 
    }

    public struct PlayerEventInfos
    {
        public readonly PlayerActions action { get; }
        public PlayerID From;
        public EventArgs infos;
        public Weapen Current;

        public PlayerEventInfos(PlayerActions action)
        {
            this.action = action;
            From = PlayerID.me;
            Current = Weapen.unknown;
            infos = null;
        }
    }
}