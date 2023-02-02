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
        Sword,
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
        protected LookState Look;
        [SerializeField]
        float movementBreakTime;
        [SerializeField]
        float fireRate;
        float nextFire;

        protected float FireRate => fireRate;
        public float MovementBreakTime => movementBreakTime;
        public WeaponType Type => WeaponType;

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
        protected void CallShootEvent()
        {
            var arg = new PlayerEventArgs(PlayerActions.OnShoot);
            EventHandler.TriggerEvent(this, arg);
        }
        public static LookState AimToLook(AimMode aim) => aim switch
        {
            AimMode.off => LookState.off,
            AimMode.ControllerStickDirection => LookState.ControllerStickMoved,
            AimMode.ControllerStickControlled => LookState.controlledExternally,
            _ => throw new System.NotImplementedException(),
        };
    }
}
