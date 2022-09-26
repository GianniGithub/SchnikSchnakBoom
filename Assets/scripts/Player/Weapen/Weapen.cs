using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class Weapon : PlayerEvent
    {
        public Transform aimCrossPrefap;
        public float fireRate;
        float nextFire;

        public Vector2 AimRange;
        protected float Range;
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
        protected void OnLooking(InputAction.CallbackContext context)
        {
            var moveInput = context.ReadValue<Vector2>();
            var t = Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y);
            Range = Mathf.Lerp(AimRange.x, AimRange.y, t);
        }
        protected Vector3 GetAimPosition(Transform trans)
        {
            return trans.forward * Range + trans.position;
        }
    }
}
