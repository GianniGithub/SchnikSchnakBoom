using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Gianni.Helper;

namespace GellosGames
{
    public class Rocket : Projectile
    {
        [SerializeField]
        float LiveTime = 3f;
        public Transform aimCrossGoal;
        Vector3[] wayPoints;

        int reachedPoints = 1;
        private ConstantForce cf;
        Vector3 nextPoint;
        void Update()
        {

            Vector3 direction = (nextPoint - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.065f);

            var dotProdukt = Vector3.Dot(transform.position - wayPoints[reachedPoints - 1], wayPoints[reachedPoints - 2] - (wayPoints[reachedPoints - 1])); //negativ if passed

            //if pass cross product
            if(dotProdukt < 0)
            {
                if (reachedPoints >= wayPoints.Length)
                {
                    //if close enough
                    if (Vector2.Distance(transform.position.ToVectorXZ(), nextPoint.ToVectorXZ()) < 0.45f)
                        TriggerExplosion();
                }
                else
                {
                    nextPoint = wayPoints[reachedPoints++];
                }
            }
        }
    

        void Start()
        {
            rb.detectCollisions = false;
            this.InvokeWait(0.35f, () => { rb.detectCollisions = true; });
            this.InvokeWait(LiveTime, () => { TriggerExplosion(); });

            var path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, aimCrossGoal.position, NavMesh.AllAreas, path))
            {
                wayPoints = path.corners;
                cf = GetComponent<ConstantForce>();
                cf.relativeForce = Vector3.forward * (rb.mass * speed);

                for (int i = 0; i < wayPoints.Length; i++)
                {
                    wayPoints[i] = wayPoints[i] + new Vector3(0f, 0.3f, 0f);
                }

                nextPoint = wayPoints[reachedPoints++];

            }
            else
            {
                Debug.LogWarning("No Path Data!");
            }


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
            var exp = new ExplosionArgs(OwnerId, transform.position, 2f, Weapen.Rocket, explosionPrefap, explosion);
            Explosion.CreateExplosion(exp);
            Destroy(gameObject);
        }


    } 
}
