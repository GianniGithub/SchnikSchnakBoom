using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class WeaponEvents : PlayerEvent
    {
        public WeaponType Type { private set; get; }
        public override void OnSpawn()
        {
            var ControllEvents = EventHandler.ControlEvents;

            ControllEvents.Player1.Artillery.performed += OnArtillery;
            ControllEvents.Player1.MiniGun.performed += OnMiniGun;
            ControllEvents.Player1.Rocket.performed += OnRocket;
        }

        public void OnMiniGun(InputAction.CallbackContext context)
        {
            Type = WeaponType.Gun;
            var e = new PlayerEventArgs(PlayerActions.WeapenSwitch);
            EventHandler.TriggerEvent(this, e);
        }

        public void OnArtillery(InputAction.CallbackContext context)
        {
            Type = WeaponType.Artillery;
            var e = new PlayerEventArgs(PlayerActions.WeapenSwitch);
            EventHandler.TriggerEvent(this, e);
        }

        public void OnRocket(InputAction.CallbackContext context)
        {
            Type = WeaponType.Rocket;
            var e = new PlayerEventArgs(PlayerActions.WeapenSwitch);
            EventHandler.TriggerEvent(this, e);
        }
    }
}
