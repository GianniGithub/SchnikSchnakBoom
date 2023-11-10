using UnityEngine;
namespace GellosGames
{
    public class HeadToTarget : MovementAndRotation
    {
        public HeadToTarget(float smoothAngel, MonoBehaviour mother) : base(mother)
        {
            rotationAngel = smoothAngel;
        }
        public override void Update()
        {
            var source = Source;
            var dir = (Target.position - source.position);
            dir.y = source.forward.y;
            var lookRotation= Quaternion.LookRotation(dir, Vector3.up);
            RotateWithDrag(source, lookRotation);
        }

    }
}