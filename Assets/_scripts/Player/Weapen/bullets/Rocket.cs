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
        public Vector3[] wayPoints;

        int reachedPoints = 1;
        private ConstantForce cf;
        Vector3 nextPoint;
        
        //Debug
        public float Scalar;
        void Update()
        {
            Vector3 direction = (nextPoint - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.065f);

            var dotProdukt = Vector3.Dot(transform.position - wayPoints[reachedPoints - 1], wayPoints[reachedPoints - 2] - wayPoints[reachedPoints - 1]); //negativ if passed
        //Debug 
        // wayPoints[reachedPoints - 1] is a head the rocked
        // wayPoints[reachedPoints - 2] is behind the rocked
        // wayPoints[reachedPoints] is a head the next but one
            var directionNextPoint = wayPoints[reachedPoints - 1] - wayPoints[reachedPoints - 2]; // AB und F
            var directionPlayer = transform.position - wayPoints[reachedPoints - 2]; // CA
            //Scalar = Vector3.Dot(directionNextPoint, directionPlayer);

            // This is a formular to get the shortest way (orthogonal) to the wayPoints Line/trail. null point of scalar 
            var a = Vector3.Dot(directionPlayer, directionNextPoint);
            var b = Vector3.Dot(directionNextPoint, directionNextPoint);
            // result is f(x) between Waypoints
            var result = (a / b);
            var t = directionNextPoint * result + wayPoints[reachedPoints - 2];
            
            Debug.DrawLine(transform.position, t, Color.black );
            for (int i = 1; i < wayPoints.Length; i++)
            {
                Debug.DrawLine(wayPoints[i-1], wayPoints[i], Color.magenta); 
            }
            //if pass cross product
            if(dotProdukt < 0)
            {
                if (reachedPoints >= wayPoints.Length)
                {
                    //if close enough
                    if (Vector2.Distance(transform.position.ToVector2XZ(), nextPoint.ToVector2XZ()) < 0.45f)
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
                    wayPoints[i] = wayPoints[i] + new Vector3(0f, 0.8f, 0f);
                }

                nextPoint = wayPoints[reachedPoints++];

            }
            else
            {
                enabled = false;
                reachedPoints++;
                nextPoint = aimCrossGoal.position;
                wayPoints = new Vector3[] { nextPoint, transform.position, nextPoint };
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
            var exp = new ExplosionArgs(OwnerId, transform.position, 2f, WeaponType.Rocket, explosionPrefap, explosion, DamagePower);
            Explosion.CreateExplosion(exp);
            Destroy(gameObject);
        }


    } 
}
