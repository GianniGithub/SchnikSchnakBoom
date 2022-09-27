using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gianni.Helper;

namespace GellosGames
{
    public enum AimMode { off, start, accurate}
    public abstract class Weapon : PlayerEvent
    {
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
