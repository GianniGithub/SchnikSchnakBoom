using System;
using UnityEngine;
namespace GellosGames
{
    [Serializable]
    public class HeadToTarget : Rotation
    {
        public HeadToTarget()
        {
            
        }
        public HeadToTarget(MonoBehaviour mother) : base(mother)
        {
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