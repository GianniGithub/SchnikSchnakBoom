using UnityEngine;
namespace GellosGames
{
    [RequireComponent(typeof(ConstantForce))]
    public class SmartChase : NPCMode, ITarget<ClosestPlayerNavMeshPath>, IPathEvents
    {
        private HeadToPath moveTo;
        private TargetRanking<ClosestPlayerNavMeshPath> targetingAct;
        [SerializeField]
        private PID Gain;
        [SerializeField]
        private float rotaionAngel;
        public override void OnNPCSpawn()
        {
            CurrentActionMode = Idle.Universal;

            CurrentMovementMode = moveTo = new HeadToPath(this, Gain, rotaionAngel);
            CurrentActionMode = targetingAct = new TargetRanking<ClosestPlayerNavMeshPath>(this, 1f);
        }
        public void TargetUpdate(ClosestPlayerNavMeshPath target)
        {
            moveTo.CalculatePath(target.Player);
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
                Debug.Log("SMART NPC: Cant find Player!");
                moveTo.ForceMover.enabled = false;
                CurrentMovementMode = Idle.Universal;
                
                // TODO Try in 20 sek again?
            }
        }
    }
}