using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Blade : PlayerEvent
    {
        [SerializeField]
        float damage = 20f;
        [SerializeField]
        float force = 3;
        [SerializeField]
        private Rigidbody rb;
        List<ContactPoint> contactPoints = new List<ContactPoint>();

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            enabled = false;
        }
        public override void OnSpawn()
        {
            //EventHandler.StartListening(PlayerActions.VehicleStateChange, VehicleStateChange);
        }
        private void OnEnable()
        {
            rb.detectCollisions = true;
        }
        private void OnDisable()
        {
            rb.detectCollisions = false;
        }
        private void OnCollisionEnter(Collision coli)
        {
            if(coli.rigidbody != null)
            {
                float currentForce = force;

                if (coli.gameObject.tag == "Player")
                {
                    Debug.Log("Hit Player");

                    var events = PlayerEvents.GetPlayerEventsHandler(coli.gameObject);
                    var eA = new HitArgs(EventHandler, coli.GetContact(0).point, WeaponType.Sword);
                    eA.Damage = damage;
                    events.TriggerEvent(null, new PlayerEventArgs(PlayerActions.OnDamage, eA));

                    // as more damage the player have as 3 x more damgeforce
                    currentForce = force + (eA.PlayerPoints.Damage / 10);
                }
                else
                {
                    Debug.Log("Hit Obstical");
                }

                int length = coli.GetContacts(contactPoints);
                for (int i = 0; i < length; i++)
                {
                    Vector3 dir = (contactPoints[i].point - transform.position).normalized;
                    coli.rigidbody.AddForce(dir * currentForce, ForceMode.Impulse);
                }
            }
        }
        //private void OnCollisionStay(Collision coli)
        //{
        //    var otherCollider = coli.GetContact(0).otherCollider;
        //    foreach (var cp in contactPoints)
        //    {
        //        if(cp.otherCollider == otherCollider)
        //        {
        //            Debug.Log("match other: " + otherCollider.gameObject.name);
        //            return;
        //        }
        //    }

        //    Debug.Log("match not: " + coli.gameObject.name);
        //    OnCollisionEnter(coli);
        //}

    }
}
