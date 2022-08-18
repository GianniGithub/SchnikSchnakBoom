using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;

public class PlayersControlls : MonoBehaviour, IPlayer1Actions
{
    public event Action<bool> OnLookStateSwitch;
    public float Speed;
    public Vector2 PowerRange;
    Vector3 nextMove;
    Rigidbody rb;
    ArtilleriePath aPath;

    public void OnMainShoot(InputAction.CallbackContext context)
    {
        Debug.Log("X Button");
    }

    public void OnMovment(InputAction.CallbackContext context)
    {
        var moveInput = context.ReadValue<Vector2>();
        nextMove = new Vector3(moveInput.x, 0, moveInput.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aPath = GetComponentInChildren<ArtilleriePath>();
        var test = new @PlayerController();
        test.Player1.MainShoot.performed += OnMainShoot;

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
            OnLookStateSwitch(false);
            return;
        }
        else
        {
            OnLookStateSwitch(true);
        }

        float heading = Mathf.Atan2(dPad.x, dPad.y);
        transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f);

        var t = Mathf.Abs(dPad.x) + Mathf.Abs(dPad.y);
        aPath.Power = Mathf.Lerp(PowerRange.x, PowerRange.y, t);
    }
}
