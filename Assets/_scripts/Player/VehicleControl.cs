using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gianni.Helper;

namespace GellosGames
{
    public enum VehicleState
    {
        IsDriving,
        InAir,
        uncontrollable,
        Idle
    }
    public class VehicleControl : PlayerEvent
    {
        [SerializeField]
        float Speed;
        [SerializeField]
        VehicleState vehicleState = VehicleState.Idle;
        Vector3 nextMove;
        Rigidbody rb;
        InputAction moveAction;
        GroundWheels plyGroundWheels;
        bool inBreak;
        Coroutine BreakRoutine;
        public VehicleState VehicleState
        {
            private set
            {
                vehicleState = value;
                var AimModeEventArg = new PlayerEventArgs(PlayerActions.VehicleStateChange);
                EventHandler.TriggerEvent(this, AimModeEventArg);
            }
            get => vehicleState;
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        public override void OnSpawn()
        {
            moveAction = EventHandler.ControlEvents.Player1.movment;
            EnableVehicle();
            EventHandler.StartListening(PlayerActions.OnShoot, OnShoot);
            EventHandler.StartListening(PlayerActions.OnAimModeChange, OnAimModeChange);

            plyGroundWheels = GetComponentInChildren<GroundWheels>();
            plyGroundWheels.IsGroundedEvent += PlyGroundWheels_IsGroundedEvent;
        }
        void FixedUpdate()
        {
            var moveInput = moveAction.ReadValue<Vector2>();
            nextMove = new Vector3(moveInput.x, 0, moveInput.y);

            if (VehicleState == VehicleState.IsDriving)
            {
                rb.AddForce(nextMove * Speed, ForceMode.Impulse);
                //rb.MovePosition(nextMove * Speed + transform.position);
            }

            nextMove = Vector3.zero;
        }
        private void PlyGroundWheels_IsGroundedEvent(bool isGrounded)
        {
            if (!isGrounded)
            {
                VehicleState = VehicleState.InAir;
            }
            else if (moveAction.inProgress)
                VehicleState = VehicleState.IsDriving;
            else
                VehicleState = VehicleState.Idle;
        }

        void EnableVehicle()
        {
            moveAction.performed += MoveAction_performed;
            moveAction.canceled += MoveAction_canceled; ;
            enabled = true;

            if (moveAction.inProgress)
                VehicleState = VehicleState.IsDriving;
            else
                VehicleState = VehicleState.Idle;
        }
        void DisableVehicle()
        {
            moveAction.performed -= MoveAction_performed;
            moveAction.canceled -= MoveAction_canceled;
            enabled = false;
            VehicleState = VehicleState.uncontrollable;
        }
        private void MoveAction_canceled(InputAction.CallbackContext obj)
        {
            VehicleState = VehicleState.Idle;
        }

        private void MoveAction_performed(InputAction.CallbackContext obj)
        {
            VehicleState = VehicleState.IsDriving;
        }

        private void OnAimModeChange(MonoBehaviour sender, PlayerEventArgs arg1)
        {
            LongRangeWeapon weapon = (LongRangeWeapon)sender;
            switch (weapon.AimModeState)
            {
                case AimMode.off when !inBreak && !enabled:
                    EnableVehicle();
                    break;
                case AimMode.ControllerStickDirection when !inBreak && !enabled:
                    EnableVehicle();
                    break;
                case AimMode.ControllerStickControlled:
                    DisableVehicle();
                    break;
                default:
                    break;
            }
        }
        private void OnShoot(MonoBehaviour sender, PlayerEventArgs arg)
        {
            var weapon = (Weapon)sender;

            if (weapon.MovementBreakTime == 0f)
                return;

            if (BreakRoutine != null)
            {
                StopCoroutine(BreakRoutine);
            }

            DisableVehicle();
            inBreak = true;

            BreakRoutine = this.InvokeWait(weapon.MovementBreakTime, ()=>
                {
                    inBreak = false;

                    switch (weapon.Type)
                    {
                        case WeaponType.Artillery:
                        case WeaponType.Rocket:
                            if (((LongRangeWeapon)sender).AimModeState != AimMode.ControllerStickControlled)
                            {
                                EnableVehicle();
                            }
                            return;

                        default:
                            EnableVehicle();
                            break;
                    }
                });
        }

    }

}
