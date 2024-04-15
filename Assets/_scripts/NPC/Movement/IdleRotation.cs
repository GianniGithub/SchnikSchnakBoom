using UnityEngine;
namespace GellosGames
{
    public class IdleRotation : Rotation
    {
        public IdleRotation(ConstantForce forceMover, MonoBehaviour mother) : base(mother)
        {
            forceMover.enabled = false;
        }
        public override void Update()
        {
            // Do nothing
        }
    }
}