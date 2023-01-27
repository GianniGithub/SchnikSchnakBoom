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
        LookState LookState = LookState.off;
        Transform lookTarget;
        // Start is called before the first frame update
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
                    LookState = LookState.ControllerStickMoved;
                    break;
                case AimMode.ControllerStickControlled:
                    lookTarget = args.AimCross;
                    LookState = LookState.controlledExternally;
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
            bool StickMoved = (target != Vector2.zero);

            LookState NewState = LookState.noState;
            switch (LookState)
            {
                case LookState.off when StickMoved:
                    NewState = LookState.ControllerStickMoved;
                    break;
                case LookState.ControllerStickMoved when !StickMoved:
                    NewState = LookState.off;
                    break;
                case LookState.controlledExternally when !StickMoved:
                    NewState = LookState.off;
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
                var e = new PlayerEventArgs(PlayerActions.OnLookStateChange, NewState);
                EventHandler.TriggerEvent(this, e);
                LookState = NewState;
            }
        }

        private Quaternion GetTowerRotation(Vector2 target)
        {
            if (LookState != LookState.controlledExternally)
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
