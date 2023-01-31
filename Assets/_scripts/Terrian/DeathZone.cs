using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider col)
        {
            if(col.tag == "Player")
            {
                var vehicle = col.GetComponentInChildren<VehicleControl>();
                var pe = vehicle.EventHandler;

                var e = new DeathZoneArgs(col, pe);
                GameEvents.Instance.TriggerEvent(this, new GameEventArgs(GameActions.OnPlayerDied, e));

                var arg = new PlayerEventArgs(PlayerActions.OnKilled);
                pe.TriggerEvent(this, arg);
            }
        }
    }
    public class DeathZoneArgs : EventArgs
    {
        public DeathZoneArgs(Collider playerObj, PlayerEvents pe)
        {
            PlayerObj = playerObj;
            Pe = pe;
        }
        public Collider PlayerObj { get; }
        public PlayerEvents Pe { get; }
    }
}
