using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Gianni.Helper;
using NaughtyAttributes;

namespace GellosGames
{
    public class Rocket : Projectile, IPathEvents
    {
        [ReadOnly]
        public Transform aimCrossGoal;
        [SerializeField]
        float RotationAngel = 0.065f;
        private HeadToPath pathFinding;
        [SerializeField]
        float LiveTime = 5f;
        private ConstantForce cf;
        [SerializeField]
        private PID gain;

        void Update()
        {
            pathFinding.Update();
            
            //if close enough
            if (Vector2.Distance(transform.position.ToVector2XZ(), aimCrossGoal.position.ToVector2XZ()) < 0.55f)
                TriggerExplosion();
        }
        void Start()
        {
            rb.detectCollisions = false;
            this.InvokeWait(0.35f, () => { rb.detectCollisions = true; });
            this.InvokeWait(LiveTime, () => { TriggerExplosion(); });

            pathFinding = new HeadToPath(this, aimCrossGoal, gain, RotationAngel, this.transform);
            pathFinding.CalculatePath();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!alreadyHit)
            {
                TriggerExplosion();
                alreadyHit = true;
            }
        }

        private void TriggerExplosion()
        {
            var exp = new ExplosionArgs(OwnerId, transform.position, 2f, WeaponType.Rocket, explosionPrefap, explosion, DamagePower);
            Explosion.CreateExplosion(exp);
            Destroy(gameObject);
        }
        
        public void OnPassedWaypoint(int waypointsLeft) { }
        public void OnEndOfWaypoints()
        {
            // If pass set Destroy timer to 2 sec to avoid circle loop
            this.InvokeWait(1.3f, () => { TriggerExplosion(); });
        }
        public void OnPathIsCalculated(bool reachable)
        {
            if (reachable)
            {
                cf = GetComponent<ConstantForce>();
                cf.relativeForce = Vector3.forward * (rb.mass * speed);
            }
            else
            {
                enabled = false;
            }
        }
    } 
}
