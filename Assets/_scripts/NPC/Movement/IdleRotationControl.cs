using UnityEngine;
namespace GellosGames
{
    public class IdleRotationControl : RotationControl
    {
        public IdleRotationControl(ConstantForce forceMover, MonoBehaviour mother) : base(mother)
        {
            forceMover.enabled = false;
        }
        public override void Update()
        {
            // Do nothing
        }
    }
}