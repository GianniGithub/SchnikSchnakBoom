using UnityEngine;
namespace GellosGames
{
    [RequireComponent(typeof(ConstantForce))]
    public class SmartChase : NPCMode, ITarget<ClosestPlayerNavMeshPath>, IPathEvents
    {
        private HeadToPath moveToPath;
        private HeadToTarget moveToPlayer;
        private TargetSelection<ClosestPlayerNavMeshPath> targetingAct;
        [SerializeField]
        private PID Gain;
        [SerializeField]
        private float rotaionAngel;
        public override void OnNPCSpawn()
        {
            CurrentActionMode = Idle.Universal;
            ActionState = NPCModeState.idle;
            
            CurrentMovementMode = moveToPath = new HeadToPath(this, Gain, rotaionAngel);
            MovementState = NPCModeState.followPath;
            
            CurrentBonusMode = targetingAct = new TargetSelection<ClosestPlayerNavMeshPath>(this, 1f);
            BonusState = NPCModeState.playerSelection;
        }
        public void TargetUpdate(ClosestPlayerNavMeshPath target)
        {
            moveToPath.CalculatePath(target.Player);
        }
        public void OnPassedWaypoint(int waypointsLeft)
        {
        }
        public void OnEndOfWaypoints()
        {
            moveToPlayer = new HeadToTarget(rotaionAngel, this);
            moveToPlayer.Target = targetingAct.Closest.Player;
            CurrentMovementMode = moveToPlayer;
            MovementState = NPCModeState.chasing;
        }
        public void OnPathIsCalculated(bool reachable)
        {
            if (!reachable)
            {
                Debug.Log("SMART NPC: Cant find Player!");
                moveToPath.ForceMover.enabled = false;
                CurrentMovementMode = Idle.Universal;
                MovementState = NPCModeState.idle;
                
                // TODO Try in 20 sek again?
            }
        }
    }
}