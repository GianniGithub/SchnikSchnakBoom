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
    public class Rocket : Projectile
    {
        [ReadOnly]
        public Transform aimCrossGoal;
        [ReadOnly]
        public Vector3[] wayPoints;
        [SerializeField]
        float RotationAngel = 0.065f;
        
        
        float LiveTime = 5f;
        int reachedPoints = 1;
        private ConstantForce cf;
        Vector3 nextPoint;
        bool passedLastWaypoint;

        [ReadOnly]
        [SerializeField]
        private float winkelToLine;
        [SerializeField]
        private float proportionalGain;
        [SerializeField]
        private float derivativeGain;
        private float errorLast;
        private float integrationStored;
        [SerializeField]
        private float integralGain;
        [ReadOnly]
        [SerializeField]
        private float PID;

        void Update()
        {
            Vector3 rocketPosition = transform.position;
            Quaternion lookRotation;
            
            //if pass cross product
            var crossCheckAngle = Vector3.Dot((rocketPosition - nextPoint).normalized, (wayPoints[reachedPoints - 2] - nextPoint).normalized); //negativ if passed
            if(crossCheckAngle < 0)
            {
                // If end of waypoints
                if (reachedPoints >= wayPoints.Length)
                {
                    // passedLastWaypoint still false for onetime event.
                    if (!passedLastWaypoint)
                    {
                        passedLastWaypoint = true;
                        // If pass set Destroy timer to 2 sec to avoid circle loop
                        this.InvokeWait(1.3f, () => { TriggerExplosion(); });
                    }
                    //if close enough
                    if (Vector2.Distance(rocketPosition.ToVector2XZ(), nextPoint.ToVector2XZ()) < 0.55f)
                        TriggerExplosion();     
                    
                    // aim now only to last waypoint
                    lookRotation = Quaternion.LookRotation(nextPoint - rocketPosition);
                    rotateWithDrag();
                    return;
                }
                else
                {
                    nextPoint = wayPoints[reachedPoints++];
                }
            }
            
            var directionNextPoint = (nextPoint - wayPoints[reachedPoints - 2]).normalized; // AB und F
            var directionPlayer = rocketPosition - wayPoints[reachedPoints - 2]; // CA

            // This is a formular to get the shortest way (orthogonal) to the wayPoints Line/trail. null point of scalar 
            var a = Vector3.Dot(directionPlayer, directionNextPoint);
            var b = Vector3.Dot(directionNextPoint, directionNextPoint);
            // result is f(x) between Waypoints
            var fx = (a / b);

            var NextpointOnLine = (directionNextPoint * fx + wayPoints[reachedPoints - 2]);
            // Point in 0.3f in front of Waypoint line
            var NextOrthogonalWayPoint = NextpointOnLine + (directionNextPoint.normalized * 0.3f);
            var directionPointLineToRocket = (NextOrthogonalWayPoint - rocketPosition).normalized;
            var directionLineFromRocket = NextpointOnLine - rocketPosition;
            // this represents the rotation / angel between Orthogonal to Waypoint and the route course. 
            // 0 represent 90° drift from course and 1 heads to route  
            winkelToLine = Vector3.Dot(directionNextPoint, directionPointLineToRocket);
            //PID Controller to pursuit rotation (winkelToLine) to 1
            float error = 1 - winkelToLine;
            //P
            float P = proportionalGain * error;
            //D
            float errorRateOfChange = (error - errorLast) / Time.deltaTime;
            errorLast = error;
            float D = derivativeGain * errorRateOfChange;
            //I
            integrationStored += (error * Time.deltaTime);
            float I = integralGain * integrationStored;
            PID = P + I + D;
            
            lookRotation = Quaternion.LerpUnclamped(Quaternion.LookRotation(directionNextPoint),Quaternion.LookRotation(directionLineFromRocket), Mathf.Clamp(PID,0.1f,1.4f));
            rotateWithDrag();
            
            // Debug
            // wayPoints[reachedPoints - 1] and nextPoint is in front the rocked
            // wayPoints[reachedPoints - 2] is behind the rocked
            // wayPoints[reachedPoints] is in front the next but one
            Debug.DrawLine(rocketPosition,NextOrthogonalWayPoint, Color.yellow);
            Debug.DrawLine(rocketPosition, NextpointOnLine, Color.black );
            for (int i = 1; i < wayPoints.Length; i++)
            {
                Debug.DrawLine(wayPoints[i-1], wayPoints[i], Color.magenta); 
            }
            
            void rotateWithDrag()
            {
                // Add drag to rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, RotationAngel);
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
