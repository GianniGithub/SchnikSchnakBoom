using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    [RequireComponent(typeof(ConstantForce))]
    public class MeleeNPC : ConstantForceMovement, ITarget<ClosestPlayerDistances>
    {
        public Rotation moveToTarget;
        private TargetSelection<ClosestPlayerDistances> targetingAct;
        public override void OnNPCSpawn()
        {
            CurrentActionMode = Idle.Universal;
            ActionState = NPCModeState.idle;

            moveToTarget.Mother = this;
            CurrentRotationMode = moveToTarget;
            RotationState = NPCModeState.chasing;
            
            CurrentBonusMode = targetingAct = new TargetSelection<ClosestPlayerDistances>(this, 1f);
            BonusState = NPCModeState.playerSelection;
        }
        public void TargetUpdate(ClosestPlayerDistances target)
        {
            moveToTarget.Target = target.Player;
        }
    }

}
