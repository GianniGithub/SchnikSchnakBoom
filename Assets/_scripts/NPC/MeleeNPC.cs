using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public class MeleeNPC : NPCMode, IRouted<ClosestPlayerDistances>
    {
        private HeadToPlayer moveTo;
        private GetNextPlayer<ClosestPlayerDistances> targetingAct;
        public override void OnNPCSpawn()
        {
            ActionUpdateRate = 1f;
            CurrentActionMode = IdleAction.Universal;
            
            var forceMover = GetComponent<ConstantForce>();
            CurrentMovementMode = moveTo = new HeadToPlayer(forceMover, this);
            CurrentActionMode = targetingAct = new GetNextPlayer<ClosestPlayerDistances>(this);
        }
        public void RoutUpdate(ClosestPlayerDistances rout)
        {
            moveTo.Player = rout.Player;
        }
    }

}
