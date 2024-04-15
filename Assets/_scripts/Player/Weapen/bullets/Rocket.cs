using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Gianni.Helper;
using NaughtyAttributes;
using UnityEngine.Serialization;

namespace GellosGames
{
    public class Rocket : Projectile, IPathEvents
    {
        [FormerlySerializedAs("aimCrossGoal")]
        [ReadOnly]
        public Transform aimCrossTarget;
        [SerializeField]
        float RotationAngel = 0.065f;
        private Rotation pathFinder;
        [SerializeField]
        float LiveTime = 5f;
        [SerializeField]
        private PID gain;
        private ConstantForce forceMover;

        void Update()
        {
            pathFinder.Update();
            
            //if close enough
            if (Vector2.Distance(transform.position.ToVector2XZ(), aimCrossTarget.position.ToVector2XZ()) < 0.55f)
                TriggerExplosion();
        }
        void Start()
        {
            forceMover =  GetComponent<ConstantForce>();
            rb.detectCollisions = false;
            this.InvokeWait(0.35f, () => { rb.detectCollisions = true; });
            this.InvokeWait(LiveTime, () => { TriggerExplosion(); });

            var path = new HeadToPath(this, gain, RotationAngel);
            pathFinder = path;
            path.CalculatePath(aimCrossTarget);
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
        public void OnTargetReached()
        {
            // If pass set Destroy timer to 2 sec to avoid circle loop
            this.InvokeWait(1.3f, () => { TriggerExplosion(); });
        }
        public void OnPathIsCalculated(bool reachable)
        {
            if (!reachable)
            {
                var path = new HeadToTarget(this);
                path.RotationAngel = RotationAngel;
                path.Target = aimCrossTarget;
                pathFinder = path;
            }
            
            forceMover.relativeForce = Vector3.forward * (rb.mass * speed);
        }
    } 
}
