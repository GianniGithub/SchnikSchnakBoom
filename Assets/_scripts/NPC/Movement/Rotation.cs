using System;
using UnityEngine;
namespace GellosGames
{
    [Serializable]
    public abstract class Rotation : Mode
    {
        public Transform Target;
        [SerializeField]
        float rotationAngel = 0.065f;
        public float RotationAngel
        {
            get => rotationAngel;
            set => rotationAngel = value;
        }
        protected Rotation()
        {
        }
        protected Transform Source => Mother.transform;
        
        protected Rotation(MonoBehaviour mother) : base(mother)
        {
           
        }
        protected void RotateWithDrag(Transform source, Quaternion lookRotation)
        {
            source.rotation = Quaternion.Slerp(source.rotation, lookRotation, rotationAngel);
        }
    }
}