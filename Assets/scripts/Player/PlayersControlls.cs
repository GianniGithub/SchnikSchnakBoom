using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;
using Gianni.Helper;

namespace GellosGames
{
    public enum LookState
    { 
        off, 
        ControllerStickLooking,
        AimCrossControlled,
        notVehicleControlled,
        autoPilot
    }
    public class PlayersControlls : PlayerEvent, IPlayer1Actions
    {
        [SerializeField]
        float Speed;
        [SerializeField]
        Transform WeaponTower;
        public PlayerController ControllEvents { get; private set; }
        Vector3 nextMove;
        Rigidbody rb;
        InputAction moveAction;
        LookState LookState = LookState.off;
        Transform lookTarget;


        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        public override void OnSpawn(InputDevice device)
        {
            ControllEvents = new PlayerController();
            moveAction = ControllEvents.Player1.movment;

            ControllEvents.devices = new[] { device };

            ControllEvents.Player1.looking.performed += OnLooking;
            ControllEvents.Player1.Artillery.performed += OnArtillery;
            ControllEvents.Player1.MiniGun.performed += OnMiniGun;
            ControllEvents.Player1.Rocket.performed += OnRocket;
            ControllEvents.Player1.Enable();

            EventHandler.StartListening(PlayerActions.OnAimModeChange, OnAimModeChange);
        }

        private void OnAimModeChange(MonoBehaviour sender, PlayerEventArgs e)
        {
            var args = (LongRangeWeapon)sender;
            switch (args.AimModeState)
            {
                case AimMode.ControllerStickDirection:
                    LookState = LookState.ControllerStickLooking;
                    var cev = new PlayerEventArgs(PlayerActions.OnLookStateChange, LookState.ControllerStickLooking);
                    EventHandler.TriggerEvent(this, cev);
                    break;
                case AimMode.ControllerStickControlled:
                    lookTarget = args.AimCross;
                    LookState = LookState.AimCrossControlled;
                    var aev = new PlayerEventArgs(PlayerActions.OnLookStateChange, LookState.AimCrossControlled);
                    EventHandler.TriggerEvent(this, aev);
                    break;
                default:
                    break;
            }
        }

        void FixedUpdate()
        {
            var moveInput = moveAction.ReadValue<Vector2>();
            nextMove = new Vector3(moveInput.x, 0, moveInput.y);
            rb.AddForce(nextMove * Speed, ForceMode.Force);
            nextMove = Vector3.zero;

        }

        public void OnLooking(InputAction.CallbackContext context)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            // if accurate mode, look and AimMode events are controlled by someone else (weapens)
            if (LookState != LookState.AimCrossControlled)
            {
                LookState current;
                var target = context.ReadValue<Vector2>();
                if (target != Vector2.zero)
                {
                    current = LookState.ControllerStickLooking;
                }
                else
                {
                    current = LookState.off;
                }

                if (current != LookState)
                {
                    var e = new PlayerEventArgs(PlayerActions.OnLookStateChange, current);
                    EventHandler.TriggerEvent(this, e);
                    LookState = current;
                }
                float heading = Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg;
                WeaponTower.rotation = Quaternion.Euler(0f, heading, 0f);
            }
            else
            {
                var target = lookTarget.position - transform.position;
                target.y = 0; // keep only the horizontal direction
                WeaponTower.rotation = Quaternion.LookRotation(target);
            }
        }

        public void OnMainShoot(InputAction.CallbackContext context) { }
        public void OnMovment(InputAction.CallbackContext context) { }
        public void OnConform(InputAction.CallbackContext context) { }

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

        public void OnSecondShoot(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnWeapenMode(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

    } 

}
