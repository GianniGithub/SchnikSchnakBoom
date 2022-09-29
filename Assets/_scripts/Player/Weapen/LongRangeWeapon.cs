using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Gianni.Helper;

namespace GellosGames
{
    public abstract class LongRangeWeapon : Weapon
    {
        public Transform aimCrossPrefap;
        public float AimSpeed;
        public Vector2 AimRange;
        protected float Range;
        protected AimMode AimMode;
        protected Transform aimCross;
        protected PlayerController.Player1Actions PlayerControllEvents;
        private Vector2 moveInput;
        float relativeAimSpeed;
        public Transform AimCross => aimCross;
        public AimMode AimModeState => AimMode;

        protected void OnLookStateChange(MonoBehaviour sender, PlayerEventArgs e)
        {
            switch (e.LookState)
            {
                case LookState.off:
                    AimMode = AimMode.off;
                    enabled = false;
                    aimCross.gameObject.SetActive(false);
                    OnAimmodeChanged(AimMode.off);

                    var ea = new PlayerEventArgs(PlayerActions.OnAimModeChange, WeaponType);
                    EventHandler.TriggerEvent(this, ea);
                    break;

                case LookState.ControllerStickLooking when AimMode != AimMode.ControllerStickDirection:
                    AimMode = AimMode.ControllerStickDirection;
                    enabled = true;
                    aimCross.gameObject.SetActive(true);
                    OnAimmodeChanged(AimMode.ControllerStickDirection);

                    var eb = new PlayerEventArgs(PlayerActions.OnAimModeChange, WeaponType);
                    EventHandler.TriggerEvent(this, eb);
                    break;

                case LookState.AimCrossControlled:
                    break;
                case LookState.notVehicleControlled:
                    break;
                case LookState.autoPilot:
                    break;
            }

        }
        protected void OnWeapenModeAccurateStart(InputAction.CallbackContext obj)
        {
            AimMode = AimMode.ControllerStickControlled;
            var e = new PlayerEventArgs(PlayerActions.OnAimModeChange, WeaponType);
            EventHandler.TriggerEvent(this, e);
        }
        protected void OnWeapenModeAccurateCanceld(InputAction.CallbackContext obj)
        {
            AimMode = AimMode.ControllerStickDirection;
            var e = new PlayerEventArgs(PlayerActions.OnAimModeChange, WeaponType);
            EventHandler.TriggerEvent(this, e);
        }
        protected void OnLooking(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            var t = Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y);
            Range = Mathf.Lerp(AimRange.x, AimRange.y, t);
        }
        protected Vector3 GetAimPosition(Transform trans)
        {
            switch (AimMode)
            {
                case AimMode.ControllerStickControlled:
                    var target = aimCross.transform.position + moveInput.ToVectorXZ() * AimSpeed;
                    var distance = Vector2.Distance(target.ToVector2XZ(), transform.position.ToVector2XZ());
                    if(distance > AimRange.y)
                    {
                        relativeAimSpeed = Mathf.Lerp(AimSpeed, 0, distance - AimRange.y);
                        return aimCross.transform.position + moveInput.ToVectorXZ() * relativeAimSpeed;
                    }
                    if (distance < AimRange.x)
                    {
                        relativeAimSpeed = Mathf.Lerp(AimSpeed, 0, AimRange.x - distance);
                        return aimCross.transform.position + moveInput.ToVectorXZ() * relativeAimSpeed;
                    }
                    return target;
                default:
                    return trans.forward * Range + trans.position;
            }

        }
        protected void OnKilled(MonoBehaviour arg0, PlayerEventArgs arg1)
        {
            Destroy(aimCross.gameObject);
        }
        protected abstract void OnWeapenSwitch(MonoBehaviour sender, PlayerEventArgs e);
        protected abstract void OnAimmodeChanged(AimMode aimMode);
    }
    public class LongRangeWeaponArgs : EventArgs
    {
        public AimMode AimModeState { get; }
        public Transform AimCross { get; }
        public LongRangeWeaponArgs(Transform aimCross, AimMode accurate)
        {
            AimCross = aimCross;
            AimModeState = accurate;
        }
    }
}