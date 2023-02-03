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
        public override void AfterSpawn()
        {
            // Start Weapon
            Type = WeaponType.Sword;
            var e = new PlayerEventArgs(PlayerActions.WeapenSwitch);
            EventHandler.TriggerEvent(this, e);
        }
        private void enableWeaponSwitch()
        {
            isWeaponSwitchEnabled = true;
            ControllEvents.Artillery.performed += OnArtillery;
            ControllEvents.MiniGun.performed += OnMiniGun;
            ControllEvents.Rocket.performed += OnRocket;
            ControllEvents.Conform.performed += OnSword;
        }

        private void disableWeaponSwitch()
        {
            isWeaponSwitchEnabled = false;
            ControllEvents.Artillery.performed -= OnArtillery;
            ControllEvents.MiniGun.performed -= OnMiniGun;
            ControllEvents.Rocket.performed -= OnRocket;
            ControllEvents.Conform.performed -= OnSword;
        }

        private void VehicleStateChange(MonoBehaviour sender, PlayerEventArgs arg)
        {
            switch (((VehicleControl)sender).VehicleState)
            {
                case VehicleState.Idle:
                case VehicleState.IsDriving:
                case VehicleState.InAir:
                    if(!isWeaponSwitchEnabled)
                        enableWeaponSwitch();
                    break;
                case VehicleState.uncontrollable when isWeaponSwitchEnabled:
                    disableWeaponSwitch();
                    break;
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
        private void OnSword(InputAction.CallbackContext context)
        {
            Type = WeaponType.Sword;
            var e = new PlayerEventArgs(PlayerActions.WeapenSwitch);
            EventHandler.TriggerEvent(this, e);
        }
    }
}
