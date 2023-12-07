using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    [RequireComponent(typeof(ConstantForce))]
    public class MeleeNPC : NPCMode, ITarget<ClosestPlayerDistances>
    {
        private HeadToTarget moveTo;
        private TargetSelection<ClosestPlayerDistances> targetingAct;
        [SerializeField]
        private float rotaionAngel;
        public override void OnNPCSpawn()
        {
            CurrentActionMode = Idle.Universal;
            ActionState = NPCModeState.idle;

            CurrentMovementMode = moveTo = new HeadToTarget(rotaionAngel, this);
            MovementState = NPCModeState.chasing;
            
            CurrentBonusMode = targetingAct = new TargetSelection<ClosestPlayerDistances>(this, 1f);
            BonusState = NPCModeState.playerSelection;
        }
        public void TargetUpdate(ClosestPlayerDistances target)
        {
            moveTo.Target = target.Player;
        }
    }

}
