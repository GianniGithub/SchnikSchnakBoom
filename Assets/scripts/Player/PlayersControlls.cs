using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;

namespace GellosGames
{
    public enum Weapen
    {
        unknown = 0,
        none = 1,
        Gun,
        Artillery,
        Rocket,
    }
    public class PlayersControlls : PlayerEvent, IPlayer1Actions
    {
        [SerializeField]
        float Speed;
        public PlayerController ControllEvents { get; private set; }
        Vector3 nextMove;
        Rigidbody rb;
        InputAction moveAction;
        AimMode aimState = AimMode.off;
        [NonSerialized]
        public bool OverrideRotation;
        [NonSerialized]
        public bool OverrideMoving;

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

           // EventHandler.StartListening(PlayerActions.IsAiming, (s,e)=> Debug.Log("Aim: " + e.IsAiming));
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
            var target = context.ReadValue<Vector2>();
            AimMode newMode;
            if (target != Vector2.zero)
            {
                newMode = AimMode.start;
            }
            else
            {
                newMode = AimMode.off;
            }

            if (newMode != aimState)
            {
                var e = new PlayerEventArgs(PlayerActions.OnAimStateChange, newMode);
                EventHandler.TriggerEvent(this, e);
                aimState = newMode;
            }

            if (!OverrideRotation)
            {
                float heading = Mathf.Atan2(target.x, target.y);
                transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f); 
            }
        }

        public void OnMainShoot(InputAction.CallbackContext context) { }
        public void OnMovment(InputAction.CallbackContext context) { }
        public void OnConform(InputAction.CallbackContext context) { }

        public void OnMiniGun(InputAction.CallbackContext context)
        {
            var e = new PlayerEventArgs(PlayerActions.WeapenSwitch, aimState, Weapen.Gun);
            EventHandler.TriggerEvent(this, e);
        }

        public void OnArtillery(InputAction.CallbackContext context)
        {
            var e = new PlayerEventArgs(PlayerActions.WeapenSwitch, aimState, Weapen.Artillery);
            EventHandler.TriggerEvent(this, e);
        }

        public void OnRocket(InputAction.CallbackContext context)
        {
            var e = new PlayerEventArgs(PlayerActions.WeapenSwitch, aimState, Weapen.Rocket);
            EventHandler.TriggerEvent(this, e);
        }

    } 

}
