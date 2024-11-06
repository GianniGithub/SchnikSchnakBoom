using Gianni.Helper;
using UnityEngine;
namespace GellosGames
{
    public class PassWaypoints : ConstantForceMovementControl, IPathEvents
    {
        [SerializeField]
        private WayPointCollection WayPoints;
        [SerializeField]
        private HeadToPath PathPointHeading;
        public override void OnNPCSpawn()
        {
            // Nothing for first, because is only used by player
            // see Start()
        }
        private void Start()
        {
            CurrentActionNode = Idle.Universal;
            ActionState = NPCModeState.idle;
            WayPoints = new WayPointCollection(this);
            
            // Start Targeting
            OnTargetReached();
            RotationState = NPCModeState.followPath;
        }
        public void OnPassedWaypoint(int waypointsLeft)
        {
            throw new System.NotImplementedException();
        }
        public void OnTargetReached()
        {
            Transform item;
            PathPointHeading = new HeadToPath(this);
            if (WayPoints.NextWayPoint(out item))
            {
              
                PathPointHeading.CalculatePath(item);
            }
            else
            {
                //stop moving
                ActivateMoving(false);
                RotationState = NPCModeState.idle;
            }
        }
        public void OnPathIsCalculated(bool reachable)
        {
            if (!reachable)
            {
                //stop moving
                ActivateMoving(false);
                RotationState = NPCModeState.PathError;
            }
        }
    }
}