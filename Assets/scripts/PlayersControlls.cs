using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;

public class PlayersControlls : MonoBehaviour, IPlayer1Actions
{
    public void OnMainShoot(InputAction.CallbackContext context)
    {
        Debug.Log("X Button");
    }

    public void OnMovment(InputAction.CallbackContext context)
    {
        Debug.Log("movment");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
