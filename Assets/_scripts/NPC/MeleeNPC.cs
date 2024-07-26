using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    [Serializable]
    [RequireComponent(typeof(ConstantForce))]
    public class MeleeNPC : ConstantForceMovementControl, ITarget<ClosestPlayerDistances>
    {
        [SerializeReference]
        private RotationControl headToTargetMode;
        private TargetSelection<ClosestPlayerDistances> targetingAct;
        public override void OnNPCSpawn()
        {
            CurrentActionMode = Idle.Universal;
            ActionState = NPCModeState.idle;
            
            CurrentRotationMode = headToTargetMode = new HeadToTarget(this);
            RotationState = NPCModeState.chasing;
            
            CurrentBonusMode = targetingAct = new TargetSelection<ClosestPlayerDistances>(this, 1f);
            BonusState = NPCModeState.playerSelection;
        }
        public void TargetUpdate(ClosestPlayerDistances target)
        {
            headToTargetMode.Target = target.Player;
        }
    }

}
