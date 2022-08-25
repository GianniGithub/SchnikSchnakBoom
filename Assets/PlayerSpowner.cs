using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpowner : MonoBehaviour
{
    public GameObject playerPrefab;
    void Start()
    {
        //Gamepad.all[0]
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            PlayerInput.Instantiate(playerPrefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
