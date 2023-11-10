using UnityEngine;
namespace GellosGames
{
    public class IdleMovementAndRotation : MovementAndRotation
    {
        public IdleMovementAndRotation(ConstantForce forceMover, MonoBehaviour mother) : base(mother)
        {
            forceMover.enabled = false;
        }
        public override void Update()
        {
            // Do nothing
        }
    }
}