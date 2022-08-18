using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;

public class PlayersControlls : MonoBehaviour, IPlayer1Actions
{
    public event Action<bool, Vector2> OnLookStateSwitch;
    public float Speed;
    Vector3 nextMove;
    Rigidbody rb;
    public PlayerController ControllEvents;

    public void OnMainShoot(InputAction.CallbackContext context)
    {
        Debug.Log("X Button");
    }

    public void OnMovment(InputAction.CallbackContext context)
    {
        var moveInput = context.ReadValue<Vector2>();
        nextMove = new Vector3(moveInput.x, 0, moveInput.y);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ControllEvents = new PlayerController();
        ControllEvents.Player1.Enable();
        ControllEvents.Player1.movment.performed += OnMovment;
        ControllEvents.Player1.looking.performed += OnLooking;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        rb.AddForce(nextMove * Speed, ForceMode.Force);
    }

    public void OnLooking(InputAction.CallbackContext context)
    {
        var dPad = context.ReadValue<Vector2>();
        if (dPad == Vector2.zero)
        {
            OnLookStateSwitch(false, dPad);
            return;
        }
        else
        {
            OnLookStateSwitch(true, dPad);
        }

        float heading = Mathf.Atan2(dPad.x, dPad.y);
        transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f);


    }
}
