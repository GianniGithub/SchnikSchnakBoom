using System;
using System.Collections.Generic;
using UnityEngine;

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
        OnKilled = 302,

        OnHit = 501,
        OnDamage = 502,

        OnShoot = 601,

        PlayerControllerEventsRegisterd = 701,
    }
    public class PlayerEvents : IEventComponent<PlayerEvent>
    {
        static Dictionary<GameObject, PlayerID> playerDict = new Dictionary<GameObject, PlayerID>();
        static PlayerEvents[] allPlayerEvents = new PlayerEvents[4];

        int deviceID;
        List<PlayerEvent> playerEventListener = new List<PlayerEvent>();
        GameObject player;
        PlayerID id;

        public PlayerEvents(PlayerID playerID, int deviceID, GameObject gameObject)
        {
            id = playerID;
            player = gameObject;
            this.deviceID = deviceID;

            gameObject.GetComponentsInChildren(true, playerEventListener);

            foreach (var col in playerEventListener)
            {
                col.OnSpawn();
            }
        }

        public static void AddPlayer(PlayerID playerID, int deviceID, GameObject gameObject)
        {
            var thisEventHandler = new PlayerEvents(playerID, deviceID, gameObject);

            allPlayerEvents[(int)playerID] = thisEventHandler;
            playerDict.Add(gameObject, playerID);
        }
        public static void RemovePlayer(PlayerID playerID, GameObject gameObject)
        {
            allPlayerEvents[((int)playerID)] = null;
            playerDict.Remove(gameObject);
        }
        public static void CallPlayerAction(MonoBehaviour sender, PlayerActions action, PlayerEventInfos e)
        {
            var from = playerDict[sender.gameObject];
            e.From = from;

            switch (e.To)
            {
                case PlayerID.A:
                case PlayerID.B:
                case PlayerID.C:
                case PlayerID.D:
                    foreach (var item in playerEvents[(int)e.To])
                    {
                        item.OnPlayerActionEvent(sender, action, e);
                    }
                    break;

                case PlayerID.all:
                    for (int i = 0; i < playerEvents.Length; i++)
                    {
                        foreach (var item in playerEvents[i])
                        {
                            if (item == null)
                                continue;

                            item.OnPlayerActionEvent(sender, action, e);
                        }
                    }
                    break;

                case PlayerID.me:
                    foreach (var item in playerEvents[(int)from])
                    {
                        item.OnPlayerActionEvent(sender, action, e);
                    }
                    break;

                default:
                    break;
            }
        }

        public void RegisterParentListener(PlayerID id, PlayerEvent playerEventComponent)
        {
            switch (id)
            {
                case PlayerID.A:
                case PlayerID.B:
                case PlayerID.C:
                case PlayerID.D:
                    allPlayerEvents[(int)id].RegisterListener(playerEventComponent);
                    break;
                case PlayerID.all:
                    for (int i = 0; i < allPlayerEvents.Length; i++)
                    {
                        allPlayerEvents[i].RegisterListener(playerEventComponent);
                    }
                    break;
                case PlayerID.me:
                    Debug.LogError("Use Un-/RegisterListener instat");
                    break;
                case PlayerID.testPlayer:
                    break;
                default:
                    Debug.LogError("Use RegisterListener instat");
                    break;
            }
        }
        public void UnRegisterParentListener(PlayerID id, PlayerEvent playerEventComponent)
        {
            switch (id)
            {
                case PlayerID.A:
                case PlayerID.B:
                case PlayerID.C:
                case PlayerID.D:
                    allPlayerEvents[(int)id].UnregisterListener(playerEventComponent);
                    return;

                case PlayerID.all:
                    for (int i = 0; i < allPlayerEvents.Length; i++)
                    {
                        allPlayerEvents[i].UnregisterListener(playerEventComponent);
                    }
                    return;

                default:
                    Debug.LogError("Use RegisterListener instat");
                    break;
            }
        }
        public void RegisterListener(PlayerEvent playerEventComponent)
        {
            playerEventListener.Add(playerEventComponent);
        }
        public void UnregisterListener(PlayerEvent playerEventComponent)
        {
            playerEventListener.Remove(playerEventComponent);
        }
    }
    public abstract class PlayerEvent : MonoBehaviour, IEventLeaf<PlayerEventInfos>
    {
        PlayerEvents eventParent;

        public IEventComponent<IEventLeaf<PlayerEventInfos>> EventParent => (IEventComponent<IEventLeaf<PlayerEventInfos>>)eventParent;

        IEventComponent<IEventLeaf<PlayerEventInfos>> IEventLeaf<PlayerEventInfos>.EventParent => throw new NotImplementedException();

        public virtual void OnSpawn() { }
        public virtual void Initialisation() { }
        protected virtual void OnDestroy()
        {
            EventParent.UnregisterListener(this);
        }
        public abstract void OnPlayerActionEvent(MonoBehaviour sender, PlayerActions action, PlayerEventInfos e);

        public virtual void OnEvent(MonoBehaviour sender, PlayerEventInfos e) { }

    }

    public class PlayerEventInfos : EventArgs
    {
        public PlayerActions action;
        public PlayerID From = PlayerID.me;
        public PlayerID To = PlayerID.me;

        public Weapen Current = Weapen.unknown;

        public PlayerEventInfos(PlayerActions action)
        {
            this.action = action;
        }
    }
}