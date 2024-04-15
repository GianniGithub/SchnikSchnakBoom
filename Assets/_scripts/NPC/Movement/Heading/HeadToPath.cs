using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
namespace GellosGames
{
    [Serializable]
    public class HeadToPath : Rotation
    {
        [ReadOnly]
        public Vector3[] wayPoints;
        Vector3 nextPoint;
        bool passedLastWaypoint;
        int reachedPoints = 1;
        private float winkelToLine;
        private float errorLast;
        private float integrationStored;
        private float pid;
        [SerializeField]
        private PID gain;
        private IPathEvents listener;
        private NavMeshPath pathToRun;

        public HeadToPath(MonoBehaviour Mother) : base(Mother)
        {
            listener = (IPathEvents)Mother;
        }
        public HeadToPath(MonoBehaviour Mother, PID gain, float rotationAngel) : base(Mother)
        {
            this.listener = (IPathEvents)Mother;
            this.gain = gain;
        }
        public void CalculatePath(Transform target)
        {
            this.Target = target;
            reachedPoints = 1;

            pathToRun ??= new NavMeshPath();
            if (NavMesh.CalculatePath(Source.position, Target.position, NavMesh.AllAreas, pathToRun))
            {
                wayPoints = pathToRun.corners;
                for (int i = 0; i < wayPoints.Length; i++)
                {
                    wayPoints[i] += new Vector3(0f, 0.8f, 0f);
                }
                nextPoint = wayPoints[reachedPoints++];
                listener.OnPathIsCalculated(true);
            }
            else
            {
                reachedPoints++;
                nextPoint = Target.position;
                wayPoints = new Vector3[] {  Source.position, Source.position, nextPoint };
                listener.OnPathIsCalculated(false);
            }
        }
        public override void Update()
        {
            Vector3 rocketPosition = Source.position;
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
                        listener.OnTargetReached();
                    }
                    
                    // aim now only to last waypoint
                    lookRotation = Quaternion.LookRotation(nextPoint - rocketPosition);
                    RotateWithDrag(Source,lookRotation);
                    return;
                }
                else
                {
                    listener.OnPassedWaypoint(wayPoints.Length - reachedPoints);
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
            //pid Controller to pursuit rotation (winkelToLine) to 1
            float error = 1f - winkelToLine;
            //Proportional
            float P = gain.Proportional * error;
            //Derivative
            float errorRateOfChange = (error - errorLast) / Time.deltaTime;
            errorLast = error;
            float D = gain.Derivative * errorRateOfChange;
            //Integral
            integrationStored += (error * Time.deltaTime);
            float I = gain.Integral * integrationStored;
            pid = P + I + D;
            
            lookRotation = Quaternion.LerpUnclamped(Quaternion.LookRotation(directionNextPoint),Quaternion.LookRotation(directionLineFromRocket), Mathf.Clamp(pid,-0.8f,1.4f));
            RotateWithDrag(Source, lookRotation);
            
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
            

        }
    }
    public interface IPathEvents
    {
        void OnPassedWaypoint(int waypointsLeft);
        void OnTargetReached();
        void OnPathIsCalculated(bool reachable);
    }
    [Serializable]
    public struct PID
    {
        [FormerlySerializedAs("P")]
        public float Proportional;
        [FormerlySerializedAs("I")]
        public float Integral;
        [FormerlySerializedAs("D")]
        public float Derivative;
    }
}