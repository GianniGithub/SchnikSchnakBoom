using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class WeaponEvents : PlayerEvent
    {
        public override void OnSpawn()
        {
            var ControllEvents = EventHandler.ControlEvents;

            ControllEvents.Player1.Artillery.performed += OnArtillery;
            ControllEvents.Player1.MiniGun.performed += OnMiniGun;
            ControllEvents.Player1.Rocket.performed += OnRocket;
        }

        public void OnMiniGun(InputAction.CallbackContext context)
        {
            var e = new PlayerEventArgs(PlayerActions.WeapenSwitch, LookState.off, WeaponType.Gun);
            EventHandler.TriggerEvent(this, e);
        }

        public void OnArtillery(InputAction.CallbackContext context)
        {
            var e = new PlayerEventArgs(PlayerActions.WeapenSwitch, LookState.off, WeaponType.Artillery);
            EventHandler.TriggerEvent(this, e);
        }

        public void OnRocket(InputAction.CallbackContext context)
        {
            var e = new PlayerEventArgs(PlayerActions.WeapenSwitch, LookState.off, WeaponType.Rocket);
            EventHandler.TriggerEvent(this, e);
        }
    }
}
