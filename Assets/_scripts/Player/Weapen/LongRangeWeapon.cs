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
        [SerializeField]
        Vector2 aimRange;
        [SerializeField]
        protected AimMode AimMode;
        public Transform aimCrossPrefap;
        public float AimSpeed;
        protected float Range;
        protected Transform aimCross;
        protected PlayerController.Player1Actions PlayerControllEvents;
        private Vector2 moveInput;
        public Transform AimCross => aimCross;
        public AimMode AimModeState => AimMode;
        protected Vector2 AimRange => aimRange;

        protected void OnLookStateChange(MonoBehaviour sender, PlayerEventArgs e)
        {
            Look = ((TowerControl)sender).LookState;

            if (gameObject.activeInHierarchy)
            {
                SetWeaponAimMode();
            }
        }
        protected void SetWeaponAimMode()
        {
            switch (Look)
            {
                case LookState.off:
                    AimMode = AimMode.off;
                    enabled = false;
                    aimCross.gameObject.SetActive(false);
                    OnAimmodeChanged(AimMode.off);
                    break;

                case LookState.ControllerStickMoved:
                    AimMode = AimMode.ControllerStickDirection;
                    enabled = true;
                    aimCross.gameObject.SetActive(true);
                    OnAimmodeChanged(AimMode.ControllerStickDirection);
                    break;
            }
        }
        protected void OnWeapenModeAccurateStart(InputAction.CallbackContext obj)
        {
            AimMode = AimMode.ControllerStickControlled;
            OnAimmodeChanged(AimMode.ControllerStickControlled);
        }
        protected void OnWeapenModeAccurateCanceld(InputAction.CallbackContext obj)
        {
            AimMode = AimMode.ControllerStickDirection;
            OnAimmodeChanged(AimMode.ControllerStickDirection);
        }
        protected void CheckIfWeapenModeAlreadyPerformed()
        {
            if (PlayerControllEvents.WeapenMode.inProgress)
            {
                AimMode = AimMode.ControllerStickControlled;
                OnAimmodeChanged(AimMode.ControllerStickControlled);
            }
        }
        protected void OnLooking(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            var t = Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y);
            Range = Mathf.Lerp(aimRange.x, aimRange.y, t);
        }
        protected Vector3 GetAimPosition(Transform trans)
        {
            switch (AimMode)
            {
                case AimMode.ControllerStickControlled:
                    var aimRange = GetAimRange();
                    var target = aimCross.transform.position + moveInput.ToVectorXZ() * AimSpeed;
                    var distance = Vector2.Distance(target.ToVector2XZ(), transform.position.ToVector2XZ());
                    if (distance > aimRange.y)
                    {
                        var direction = (target - transform.position).normalized;
                        return (direction * aimRange.y) + transform.position;
                    }
                    if (distance < aimRange.x)
                    {
                        var direction = (target - transform.position).normalized;
                        return (direction * aimRange.x) + transform.position;
                    }
                    return target;
                case AimMode.ControllerStickDirection:
                    return trans.forward * Range + trans.position;
                default:
                    return trans.position;
            }

        }
        protected void OnKilled(MonoBehaviour arg0, PlayerEventArgs arg1)
        {
            Destroy(aimCross.gameObject);
        }
        protected abstract void OnWeapenSwitch(MonoBehaviour sender, PlayerEventArgs e);
        protected virtual void OnAimmodeChanged(AimMode aimMode)
        {
            var AimModeEventArg = new PlayerEventArgs(PlayerActions.OnAimModeChange);
            EventHandler.TriggerEvent(this, AimModeEventArg);
        }
        protected virtual Vector2 GetAimRange() => AimRange;
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