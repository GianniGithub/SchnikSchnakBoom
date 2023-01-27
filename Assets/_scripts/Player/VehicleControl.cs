using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;
using Gianni.Helper;

namespace GellosGames
{

    public class VehicleControl : PlayerEvent
    {
        [SerializeField]
        float Speed;
        Vector3 nextMove;
        Rigidbody rb;
        InputAction moveAction;
        GroundWheels plyGroundWheels;


        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            plyGroundWheels = GetComponentInChildren<GroundWheels>();
        }
        public override void OnSpawn()
        {
            moveAction = EventHandler.ControlEvents.Player1.movment;
        }

        void FixedUpdate()
        {
            var moveInput = moveAction.ReadValue<Vector2>();
            nextMove = new Vector3(moveInput.x, 0, moveInput.y);

            if (plyGroundWheels.IsGrounded)
            {
                rb.AddForce(nextMove * Speed, ForceMode.Impulse);
                //rb.MovePosition(nextMove * Speed + transform.position);
            }

            nextMove = Vector3.zero; 
        }



    } 

}
