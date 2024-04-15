using UnityEngine;
namespace GellosGames
{
    public class ConstantForceMovement : NPCMode
    {
        protected readonly ConstantForce ForceMover;
        public ConstantForceMovement()
        {
            ForceMover =  GetComponent<ConstantForce>();
        }
        public void ActivateMoving(bool state) => ForceMover.enabled = state;
    }
}