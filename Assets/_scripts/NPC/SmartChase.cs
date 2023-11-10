using UnityEngine;
namespace GellosGames
{
    [RequireComponent(typeof(ConstantForce))]
    public class SmartChase: NPCMode, ITarget<ClosestPlayerNavMeshPath>, IPathEvents
    {
        private HeadToPath moveTo;
        private TargetRanking<ClosestPlayerNavMeshPath> targetingAct;
        [SerializeField]
        private PID Gain;
        [SerializeField]
        private float rotaionAngel;
        public override void OnNPCSpawn()
        {
            ActionUpdateRate = 1f;
            CurrentActionMode = IdleAction.Universal;
            
            var forceMover = GetComponent<ConstantForce>();
            CurrentActionMode = targetingAct = new TargetRanking<ClosestPlayerNavMeshPath>(this);
            CurrentMovementMode = moveTo = new HeadToPath(this, targetingAct.Closest.Player, Gain, rotaionAngel);
        }
        public void TargetUpdate(ClosestPlayerNavMeshPath target)
        {
            CurrentMovementMode = moveTo = new HeadToPath(this, target.Player, Gain, rotaionAngel);
        }
        public void OnPassedWaypoint(int waypointsLeft)
        {
        }
        public void OnEndOfWaypoints()
        {
            // Kill him
        }
        public void OnPathIsCalculated(bool reachable)
        {
            if (!reachable)
            {
                // TODO Try in 20 sek again?
            }
        }
    }
}