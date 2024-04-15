using System;
using UnityEngine;
namespace GellosGames
{
    [RequireComponent(typeof(ConstantForce))]
    public class SmartChase : ConstantForceMovement, ITarget<ClosestPlayerNavMeshPath>, IPathEvents
    {
        [SerializeField]
        private HeadToPath moveToPath;
        [SerializeField]
        private HeadToTarget moveToPlayer;
        private TargetSelection<ClosestPlayerNavMeshPath> targetingAct;
        [SerializeField]
        private PID Gain;
        [SerializeField]
        private float rotaionAngel = 0.2f;
        public override void OnNPCSpawn()
        {
            CurrentActionMode = Idle.Universal;
            ActionState = NPCModeState.idle;
            
            CurrentRotationMode = moveToPath = new HeadToPath(this, Gain, rotaionAngel);
            RotationState = NPCModeState.followPath;
            
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
        public void OnTargetReached()
        {
            moveToPlayer = new HeadToTarget(this)
            {
                Target = targetingAct.Closest.Player
            };
            CurrentRotationMode = moveToPlayer;
            RotationState = NPCModeState.chasing;
        }
        public void OnPathIsCalculated(bool reachable)
        {
            if (!reachable)
            {
                Debug.Log("SMART NPC: Cant find Player!");
                ActivateMoving(false);
                CurrentRotationMode = Idle.Universal;
                RotationState = NPCModeState.PathError;
                
                // TODO Try in 20 sek again?
            }
        }
    }
}