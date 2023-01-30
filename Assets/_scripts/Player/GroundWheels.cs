using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    [RequireComponent(typeof(Collider))]
    public class GroundWheels : MonoBehaviour
    {
        public event Action<bool> IsGroundedEvent;
        [SerializeField]
        bool grounded;
        public bool IsGrounded 
        { 
            get => grounded; 
            private set 
            {
                grounded = value;
                IsGroundedEvent(value);
            }
        }

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

    }
}
