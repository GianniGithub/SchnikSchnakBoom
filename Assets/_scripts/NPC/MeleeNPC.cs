using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public class MeleeNPC : NPCMode, ITarget<ClosestPlayerDistances>
    {
        private HeadToPlayer moveTo;
        private TargetRanking<ClosestPlayerDistances> targetingAct;
        public override void OnNPCSpawn()
        {
            ActionUpdateRate = 1f;
            CurrentActionMode = IdleAction.Universal;
            
            var forceMover = GetComponent<ConstantForce>();
            CurrentMovementMode = moveTo = new HeadToPlayer(forceMover, this);
            CurrentActionMode = targetingAct = new TargetRanking<ClosestPlayerDistances>(this);
        }
        public void TargetUpdate(ClosestPlayerDistances target)
        {
            moveTo.Player = target.Player;
        }
    }

}
