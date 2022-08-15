using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;

public class PlayersControlls : MonoBehaviour, IPlayer1Actions
{
    public float Speed;

    Vector3 nextMove;
    Rigidbody rb;


    public void OnMainShoot(InputAction.CallbackContext context)
    {
        Debug.Log("X Button");
    }

    public void OnMovment(InputAction.CallbackContext context)
    {
        var moveInput = context.ReadValue<Vector2>();
        nextMove = new Vector3(moveInput.x, 0, moveInput.y);
        Debug.Log("movment");
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        var test = new @PlayerController();
        test.Player1.MainShoot.performed += OnMainShoot;

    }

    void FixedUpdate()
    {
        rb.AddForce(nextMove * Speed, ForceMode.Force);
    }
}
