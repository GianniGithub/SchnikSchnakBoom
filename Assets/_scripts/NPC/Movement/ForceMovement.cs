using UnityEngine;
namespace GellosGames
{
    public abstract class ForceMovement : NPCModeBehaviour
    {
        private readonly ConstantForce forceMover;
        protected ForceMovement(ConstantForce forceMover, NPCMode mother) : base(mother)
        {
            this.forceMover = forceMover;
        }
    }
}