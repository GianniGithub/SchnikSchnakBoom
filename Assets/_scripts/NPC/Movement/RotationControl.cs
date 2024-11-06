using System;
using UnityEngine;
namespace GellosGames
{
    [Serializable]
    public abstract class RotationControl : Node
    {
        public Transform Target;
        [SerializeField]
        float rotationAngel = 0.065f;
        public float RotationAngel
        {
            get => rotationAngel;
            set => rotationAngel = value;
        }
        protected RotationControl()
        {
        }
        protected Transform Source => Mother.transform;
        
        protected RotationControl(MonoBehaviour mother) : base(mother)
        {
           
        }
        protected void RotateWithDrag(Transform source, Quaternion lookRotation)
        {
            source.rotation = Quaternion.Slerp(source.rotation, lookRotation, rotationAngel);
        }
    }
}