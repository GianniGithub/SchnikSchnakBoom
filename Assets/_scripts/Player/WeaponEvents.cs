using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class WeaponEvents : PlayerEvent
    {
        PlayerController.Player1Actions ControllEvents;
        bool isWeaponSwitchEnabled;
        public WeaponType Type { private set; get; }
        public override void OnSpawn()
        {
            EventHandler.StartListening(PlayerActions.VehicleStateChange, VehicleStateChange);
            ControllEvents = EventHandler.ControlEvents.Player1;

            enableWeaponSwitch();
        }

        private void enableWeaponSwitch()
        {
            isWeaponSwitchEnabled = true;
            ControllEvents.Artillery.performed += OnArtillery;
            ControllEvents.MiniGun.performed += OnMiniGun;
            ControllEvents.Rocket.performed += OnRocket;
        }
        private void disableWeaponSwitch()
        {
            isWeaponSwitchEnabled = false;
            ControllEvents.Artillery.performed -= OnArtillery;
            ControllEvents.MiniGun.performed -= OnMiniGun;
            ControllEvents.Rocket.performed -= OnRocket;
        }

        private void VehicleStateChange(MonoBehaviour sender, PlayerEventArgs arg)
        {
            var vehicle = (VehicleControl)sender;
            if(vehicle.VehicleState == VehicleState.uncontrollable && isWeaponSwitchEnabled)
            {
                disableWeaponSwitch();
            }
            else if(!isWeaponSwitchEnabled)
            {
                enableWeaponSwitch();
            }
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
