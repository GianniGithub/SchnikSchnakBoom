using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GellosGames
{
    public enum PlayerID
    {
        P1 = 0,
        P2 = 1,
        P3 = 2,
        P4 = 3,
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

        OnLookStateChange = 701,
        OnAimModeChange = 702,

        PlayerControllerEventsRegisterd = 7801,

        VehicleStateChange = 801,
    }
    public class PlayerEvents : SpownObjects<PlayerActions, PlayerEventArgs>
    {
        public PlayerController ControlEvents;
        public static int PlayerCount => playerDict.Count;
        public PlayerID id { get; }
        public GameObject PlayerObject { get; }
        public int PlayerSlot => (int)id;

        static Dictionary<GameObject, PlayerID> playerDict = new Dictionary<GameObject, PlayerID>();
        static PlayerEvents[] allPlayerEvents = new PlayerEvents[4];
        

        private PlayerEvents(PlayerID playerID, GameObject gameObject, UnityEngine.InputSystem.InputDevice device)
        {
            id = playerID;
            PlayerObject = gameObject;
            ControlEvents = new PlayerController();
            ControlEvents.devices = new[] { device };
            ControlEvents.Player1.Enable();
        }

        public static PlayerEvents AddPlayer(PlayerID playerID, GameObject gameObject, UnityEngine.InputSystem.InputDevice device)
        {
            var thisEventHandler = new PlayerEvents(playerID, gameObject, device);

            allPlayerEvents[(int)playerID] = thisEventHandler;
            playerDict.Add(gameObject, playerID);


            foreach (var col in CollectChilds<PlayerEvent>(gameObject))
            {
                col.EventHandler = thisEventHandler;
                col.OnSpawn();
            }
            thisEventHandler.StartListening(PlayerActions.OnKilled, (s, e) => RemovePlayer(e.From));

            return thisEventHandler;
        }



        public static void RemovePlayer(PlayerID playerID)
        {
            var pe = allPlayerEvents[(int)playerID];
            // All Events are wiped
            //pe.EventDictionary.Clear();
            playerDict.Remove(pe.PlayerObject);
            //Disable Inputs, On Destroy events tray to read death Gameobjects
            pe.ControlEvents.Player1.Disable();
            //with delay so that OnKilled event runns to the End 
            UnityEngine.Object.Destroy(pe.PlayerObject, 0.4f);
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
    public abstract class PlayerEvent : SpownEvent
    {
        public PlayerEvents EventHandler;
        /// <summary>
        /// After Awake and Start, when controller device is set
        /// </summary>
        public virtual void OnSpawn() { }

    }
    public struct PlayerEventArgs
    {
        public readonly PlayerActions Action { get; }
        public EventArgs EventInfos { get; }
        public PlayerID From { get; set; }

        public PlayerEventArgs(PlayerActions action)
        {
            this.Action = action;
            From = PlayerID.me;
            EventInfos = null;
        }

        public PlayerEventArgs(PlayerActions action, EventArgs e) : this(action)
        {
            EventInfos = e;
        }
    }
}