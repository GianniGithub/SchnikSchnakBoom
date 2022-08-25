using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
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
    public class PlayerEvents : IService
    {
        PlayerEvent[] player = new PlayerEvent[4];
        Dictionary<GameObject, PlayerEvent> playerDict = new Dictionary<GameObject, PlayerEvent>();
        public PlayerEvent this[GameObject index]
        {
            get => playerDict[index];
        }
        public PlayerEvent this[int index]
        {
            get => player[index];
        }
        public PlayerEvent Player(int id) => player[id];
        public PlayerEvent Player(GameObject player) => playerDict[player];
        public void AddPlayer(int playerID, int deviceID, GameObject gameObject)
        {
            var pEvent = new PlayerEvent(playerID, deviceID, gameObject);
            player[playerID] = pEvent;
            playerDict.Add(gameObject, pEvent);
        }

    }
    public class PlayerEvent : UIStageLocator<PlayerActions>
    {
        public readonly int PlayerID;
        public readonly int DeviceID;
        public readonly GameObject gameObject;

        public PlayerEvent(int playerID, int deviceID, GameObject gameObject)
        {
            PlayerID = playerID;
            DeviceID = deviceID;
            this.gameObject = gameObject;
        }

        public override void RunEvent(MonoBehaviour sender, Event<PlayerActions> e)
        {
            RunSpecificEvent(sender, e);
        }
    } 
}