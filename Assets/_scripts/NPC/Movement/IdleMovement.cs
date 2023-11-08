using UnityEngine;
namespace GellosGames
{
    public class IdleMovement : ForceMovement
    {
        public IdleMovement(ConstantForce forceMover, NPCMode mother) : base(forceMover, mother)
        {
            forceMover.enabled = false;
        }
        public override void Update()
        {
            // Do nothing
        }
    }
}