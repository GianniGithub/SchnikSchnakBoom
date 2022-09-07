using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public class Weapon : PlayerEvent
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
