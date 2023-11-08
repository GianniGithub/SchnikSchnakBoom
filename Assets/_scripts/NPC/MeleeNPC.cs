using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public class MeleeNPC : NPCMode
    {
        private IdleMovement IdleMove;
        public override void OnNPCSpawn()
        {
            CurrentActionMode = IdleAction.Universal;
            
            var forceMover = GetComponent<ConstantForce>();
            CurrentMovementMode = IdleMove = new IdleMovement(forceMover, this);
            
            var player =  PlayerEvents.GetPlayerEventsHandler(PlayerID.P1);
            player.StartListening(pla);
        }
        public override void StateChanged<TAction>(TAction newState)
        {
            throw new System.NotImplementedException();
        }
    }

}
