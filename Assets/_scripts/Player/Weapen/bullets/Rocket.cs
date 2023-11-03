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

        public Transform aimCrossGoal;
        public Vector3[] wayPoints;
        
        public float RotationAngel = 0.065f;
        public Vector2 RangeToAddAimDistance;
        public float maxDistanceToCorrect;
        public float CorrectionSensibility;
        public float distance;
        public float winkelToLine;
        
        float LiveTime = 5f;
        int reachedPoints = 1;
        private ConstantForce cf;
        Vector3 nextPoint;
        private bool passedLastWaypoint;
        public float CorrectedAngel;
        [SerializeField]
        private float proportionalGain;
        [SerializeField]
        private float derivativeGain;
        private float errorLast;
        private float integrationStored;
        [SerializeField]
        private float integralGain;
        [SerializeField]
        private float PID;

        void Update()
        {
            Vector3 rocketPosition = transform.position;
            //if pass cross product
            var crossCheckAngle = Vector3.Dot((rocketPosition - nextPoint).normalized, (wayPoints[reachedPoints - 2] - nextPoint).normalized); //negativ if passed

                        // wayPoints[reachedPoints - 1] and nextPoint is in front the rocked
                        // wayPoints[reachedPoints - 2] is behind the rocked
                        // wayPoints[reachedPoints] is in front the next but one
                        
            var directionNextPoint = (nextPoint - wayPoints[reachedPoints - 2]).normalized; // AB und F
            var directionPlayer = rocketPosition - wayPoints[reachedPoints - 2]; // CA
            //Scalar = Vector3.Dot(directionNextPoint, directionPlayer);

            // This is a formular to get the shortest way (orthogonal) to the wayPoints Line/trail. null point of scalar 
            var a = Vector3.Dot(directionPlayer, directionNextPoint);
            var b = Vector3.Dot(directionNextPoint, directionNextPoint);
            // result is f(x) between Waypoints
            var result = (a / b);

            var NextpointOnLine = (directionNextPoint * result + wayPoints[reachedPoints - 2]);
            var NextOrthogonalWayPoint = NextpointOnLine + (directionNextPoint.normalized * 0.3f);
            var directionPointLineToRocket = (NextOrthogonalWayPoint - rocketPosition).normalized;
            var directionLineFromRocket = NextpointOnLine - rocketPosition;
            winkelToLine = Vector3.Dot(directionNextPoint, directionPointLineToRocket);
            //PID Controller
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
            
            Quaternion lookRotation = Quaternion.LerpUnclamped(Quaternion.LookRotation(directionNextPoint),Quaternion.LookRotation(directionLineFromRocket), Mathf.Clamp(PID,0.1f,1f));

            
            
            
            
            distance = Vector3.Distance(NextpointOnLine, rocketPosition);
            
            
            Debug.DrawLine(rocketPosition,NextOrthogonalWayPoint, Color.yellow);
            Debug.DrawLine(rocketPosition, NextpointOnLine, Color.black );
            for (int i = 1; i < wayPoints.Length; i++)
            {
                Debug.DrawLine(wayPoints[i-1], wayPoints[i], Color.magenta); 
            }
            
            if(crossCheckAngle < 0)
            {
                if (reachedPoints >= wayPoints.Length)
                {
                    passedLastWaypoint = true;
                }
                else
                {
                    nextPoint = wayPoints[reachedPoints++];
                }
            }

            if (passedLastWaypoint)
            {
                //if close enough
                if (Vector2.Distance(rocketPosition.ToVector2XZ(), nextPoint.ToVector2XZ()) < 0.45f)
                    TriggerExplosion();     
                    
                // If pass set Destroy timer to 2 sec to avoid circle loop
                this.InvokeWait(1.3f, () => { TriggerExplosion(); });
                    
                // aim now only to last waypoint
                NextpointOnLine = nextPoint;
            }
            
            // Rotation
            //Vector3 direction =
                //(nextPoint - rocketPosition);
                //(correctedPointOnRoute - rocketPosition);
            // lookRotation =
                //lookRotation = Quaternion.LookRotation(direction);
                //Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, RotationAngel);
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
