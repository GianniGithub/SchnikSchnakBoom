using GellosGames.Assets._scripts.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public enum LookState
    {
        noState,
        off,
        ControllerStickMoved,
        controlledExternally,
        notVehicleControlled,
        autoPilot
    }
    public class TowerControl : PlayerEvent
    {
        [SerializeField]
        Transform WeaponTower;
        [SerializeField]
        LookState lookState = LookState.off;
        Transform lookTarget;

        public LookState LookState => lookState;
        public override void OnSpawn()
        {
            EventHandler.ControlEvents.Player1.looking.performed += OnLooking;
            EventHandler.StartListening(PlayerActions.OnAimModeChange, OnAimModeChange);
        }

        private void OnAimModeChange(MonoBehaviour sender, PlayerEventArgs e)
        {
            var args = (LongRangeWeapon)sender;

            switch (args.AimModeState)
            {
                case AimMode.ControllerStickDirection:
                    lookState = LookState.ControllerStickMoved;
                    break;
                case AimMode.ControllerStickControlled:
                    lookTarget = args.AimCross;
                    lookState = LookState.controlledExternally;
                    break;
                default:
                    break;
            }
        }

        public void OnLooking(InputAction.CallbackContext context)
        {
            var target = context.ReadValue<Vector2>();

            CheckForControllerStickMovings(target);

            // if accurate mode, look and AimMode events are controlled by someone else (weapens)
            WeaponTower.rotation = GetTowerRotation(target);
        }

        private void CheckForControllerStickMovings(Vector2 target)
        {
            bool StickMoved = !target.IsQuiteZero(0.07f);
            LookState NewState = LookState.noState;

            switch (lookState)
            {
                case LookState.off when StickMoved:
                    NewState = LookState.ControllerStickMoved;
                    break;
                case LookState.ControllerStickMoved when !StickMoved:
                    NewState = LookState.off;
                    break;
                case LookState.controlledExternally:
                    // Do Nothing
                    break;
                case LookState.notVehicleControlled when !StickMoved:
                    NewState = LookState.off;
                    break;
                case LookState.autoPilot when StickMoved:
                    NewState = LookState.off;
                    break;
                default:
                    break;
            }

            if (NewState != LookState.noState)
            {
                lookState = NewState;
                var e = new PlayerEventArgs(PlayerActions.OnLookStateChange);
                EventHandler.TriggerEvent(this, e);
            }
        }

        private Quaternion GetTowerRotation(Vector2 target)
        {
            if (lookState != LookState.controlledExternally)
            {
                float heading = Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg;
                return Quaternion.Euler(0f, heading, 0f);
            }
            else
            {
                var direction = lookTarget.position - transform.position;
                direction.y = 0; // keep only the horizontal direction
                return Quaternion.LookRotation(direction);
                //return Quaternion.LookRotation(rb.velocity);
            }
        }

    }
}
