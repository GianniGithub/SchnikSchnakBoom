using UnityEngine;
namespace GellosGames
{
    public abstract class MovementAndRotation : Mode
    {
        public readonly ConstantForce ForceMover;
        public Transform Target;
        [SerializeField]
        protected float rotationAngel = 0.065f;
        protected Transform Source => Mother.transform;

        protected MovementAndRotation(MonoBehaviour mother) : base(mother)
        {
            ForceMover =  mother.GetComponent<ConstantForce>();
        }
        protected void RotateWithDrag(Transform source, Quaternion lookRotation)
        {
            // Add drag to rotation
            source.rotation = Quaternion.Slerp(source.rotation, lookRotation, rotationAngel);
        }
    }
}