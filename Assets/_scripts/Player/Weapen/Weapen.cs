using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gianni.Helper;

namespace GellosGames
{
    public enum WeaponType
    {
        unknown = 0,
        none = 1,
        Gun,
        Artillery,
        Rocket,
    }
    public enum AimMode 
    {
        off, 
        ControllerStickDirection,
        ControllerStickControlled
    }
    public abstract class Weapon : PlayerEvent
    {
        protected WeaponType WeaponType;
        public float fireRate;
        float nextFire;

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
    }
}
