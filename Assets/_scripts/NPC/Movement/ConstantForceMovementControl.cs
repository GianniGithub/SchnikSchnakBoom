using System;
using UnityEngine;
namespace GellosGames
{
    public class ConstantForceMovementControl : NPCMode
    {
        protected ConstantForce ForceMover;
        private void Awake()
        {
            ForceMover =  GetComponent<ConstantForce>();
        }
        public void ActivateMoving(bool state) => ForceMover.enabled = state;
    }
}