using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
namespace GellosGames
{
[Serializable]
    public class HeadToPath : MovementAndRotation
    {
        [ReadOnly]
        public Vector3[] wayPoints;
        private readonly Transform m_Source;
        Vector3 nextPoint;
        bool passedLastWaypoint;
        int reachedPoints = 1;
        private float winkelToLine;
        [SerializeField]
        private float proportionalGain;
        [SerializeField]
        private float derivativeGain;
        private float errorLast;
        private float integrationStored;
        [SerializeField]
        private float integralGain;
        private float PID;
        private PID gain;
        private IPathEvents listener;
        
        public HeadToPath(MonoBehaviour Mother, Transform target, PID gain, float rotationAngel) : base(Mother)
        {
            this.Target = target;
            this.listener = (IPathEvents)Mother;
            this.gain = gain;
            this.rotationAngel = rotationAngel;
        }
        public void CalculatePath()
        {

            var path = new NavMeshPath();
            if (NavMesh.CalculatePath(Source.position, Target.position, NavMesh.AllAreas, path))
            {
                wayPoints = path.corners;
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
                wayPoints = new Vector3[] { nextPoint, Source.position, nextPoint };
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
                        listener.OnEndOfWaypoints();
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
            //PID Controller to pursuit rotation (winkelToLine) to 1
            float error = 1f - winkelToLine;
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
        void OnEndOfWaypoints();
        void OnPathIsCalculated(bool reachable);
    }
    [Serializable]
    public struct PID
    {
        public float P, I, D;
    }
}