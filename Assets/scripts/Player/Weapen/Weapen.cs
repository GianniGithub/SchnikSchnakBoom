using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gianni.Helper;

namespace GellosGames
{
    public enum AimMode { off, start, accurate}
    public class Weapon : PlayerEvent
    {
        public Transform aimCrossPrefap;
        public float fireRate;
        float nextFire;

        public Vector2 AimRange;
        protected float Range;
        private Vector2 moveInput;
        protected AimMode AimMode;
        protected Transform aimCross;

        protected bool IsFireTimeReady 
        { 
            get
            {
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    return true;
                }
                else
                {
                    return false;
                }
            } 
        }
        protected void AimChangeBase(bool to, PlayersControlls controlls, AimMode mode)
        {
            enabled = to;
            aimCross.gameObject.SetActive(to);

            AimMode = mode;
            switch (mode)
            {
                case AimMode.off:
                    controlls.OverrideRotation = false;
                    break;
                case AimMode.start:
                    //this.InvokeWait(1.5f, ChangeToAccurate);
                    break;
                case AimMode.accurate:
                    controlls.OverrideRotation = true;
                    break;
                default:
                    break;
            }
        }
        void ChangeToAccurate()
        {
            var e = new PlayerEventArgs(PlayerActions.OnAimStateChange, AimMode.accurate);
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
                case AimMode.start:
                    return trans.forward * Range + trans.position;
                case AimMode.accurate:
                    return trans.forward * Range + trans.position;
                    break;
                case AimMode.off:
                    return trans.forward * Range + trans.position;
                    break;
                default:
                    return trans.forward * Range + trans.position;
                    break;
            }

        }
    }
}
