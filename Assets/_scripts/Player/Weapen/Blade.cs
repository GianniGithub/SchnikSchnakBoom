using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public class Blade : PlayerEvent
    {
        [SerializeField]
        float damage = 20f;
        [SerializeField]
        float force = 1;
        public override void OnSpawn()
        {
            //EventHandler.StartListening(PlayerActions.VehicleStateChange, VehicleStateChange);
        }
        private void OnCollisionEnter(Collision col)
        {
            if(col.rigidbody != null)
            {
                if(col.gameObject.tag == "Player")
                {

                    var events = PlayerEvents.GetPlayerEventsHandler(col.gameObject);
                    var eA = new HitArgs(EventHandler, col.GetContact(0).point, WeaponType.Sword);
                    eA.Damage = damage;
                    events.TriggerEvent(null, new PlayerEventArgs(PlayerActions.OnDamage, eA));

                    // as more damage the player have as 3 x more damgeforce
                    col.rigidbody.AddForce(col.GetContact(0).normal * force * eA.PlayerPoints.Damage);
                }
                else
                {
                    col.rigidbody.AddForce(col.GetContact(0).normal * force);
                }

                
            }
        }

    }
}
