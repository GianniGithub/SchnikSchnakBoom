using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    [RequireComponent(typeof(Collider))]
    public class GroundWheels : MonoBehaviour
    {
        public bool IsGrounded { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            IsGrounded = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            IsGrounded = true;
        }
        private void OnTriggerStay(Collider other)
        {
            IsGrounded = true;
        }
        private void OnTriggerExit(Collider other)
        {
            IsGrounded = false;
        }
#if UNITY_EDITOR
        public bool grounded;
        private void Update()
        {
            grounded = IsGrounded;
        }
#endif
        //private bool CheckIfGrounded()
        //{
        //    //We raycast down 1 pixel from this position to check for a collider
        //    Vector3 positionToCheck = transform.position - new Vector3(0, relativGroundPoint, 0);
        //    var target = positionToCheck + new Vector3(0, -1, 0);
        //    Debug.DrawRay(positionToCheck, target);
        //    return Physics.Raycast(positionToCheck, target, groundraylenght);

        //    //if a collider was hit, we are grounded
        //    //return hits.Length > 0;
        //}
    }
}
