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
        protected void AimChangeBase(bool to, AimMode mode)
        {
            AimMode = mode;
            if (mode != AimMode.accurate)
            {
                enabled = to;
                aimCross.gameObject.SetActive(to);
            }
            //Debug.Log("state: " + AimMode.ToString());
        }
        protected void OnWeapenModeAccurateStart(InputAction.CallbackContext obj)
        {
            var a = new LongRangeWeaponArgs() { AimCross = aimCross };
            var e = new PlayerEventArgs(PlayerActions.OnAimStateChange, AimMode.accurate, a);
            EventHandler.TriggerEvent(this, e);
        }
        protected void OnWeapenModeAccurateCanceld(InputAction.CallbackContext obj)
        {
            var e = new PlayerEventArgs(PlayerActions.OnAimStateChange, AimMode.off);
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
                case AimMode.accurate:
                    var target = aimCross.transform.position + moveInput.ToVectorXZ() * AimSpeed;
                    return aimCross.transform.position + moveInput.ToVectorXZ() * AimSpeed;
                default:
                    return trans.forward * Range + trans.position;
            }

        }
        protected void OnKilled(MonoBehaviour arg0, PlayerEventArgs arg1)
        {
            Destroy(aimCross.gameObject);
        }
        protected abstract void onWeapenSwitch(MonoBehaviour sender, PlayerEventArgs e);
    }
    public class LongRangeWeaponArgs : EventArgs
    {
        public Transform AimCross { get; set; }
    }
}