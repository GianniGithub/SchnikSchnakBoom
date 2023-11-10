using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    [RequireComponent(typeof(ConstantForce))]
    public class MeleeNPC : NPCMode, ITarget<ClosestPlayerDistances>
    {
        private HeadToTarget moveTo;
        private TargetRanking<ClosestPlayerDistances> targetingAct;
        [SerializeField]
        private float rotaionAngel;
        public override void OnNPCSpawn()
        {
            ActionUpdateRate = 1f;
            CurrentActionMode = IdleAction.Universal;

            CurrentMovementMode = moveTo = new HeadToTarget(rotaionAngel, this);
            CurrentActionMode = targetingAct = new TargetRanking<ClosestPlayerDistances>(this);
        }
        public void TargetUpdate(ClosestPlayerDistances target)
        {
            moveTo.Target = target.Player;
        }
    }

}
