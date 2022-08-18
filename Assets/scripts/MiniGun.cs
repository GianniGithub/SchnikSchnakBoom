using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGun : MonoBehaviour
{
    public PlayersControlls Controlls;
    LineRenderer lr;
    Vector3[] points;
    private PlayerController controllEvents;
    bool FireOn = false;
    public float shootTime;
    float lastShootT;
    RaycastHit hit;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        //controlls = GetComponent<PlayersControlls>();
    }
    void Start()
    {
        lr.positionCount = 2;
        points = new Vector3[2];

        Controlls.ControllEvents.Player1.MainShoot.performed += OnShootBullet;
        Controlls.OnLookStateSwitch += Controlls_OnLookStateSwitch;
    }

    private void Controlls_OnLookStateSwitch(bool arg1, Vector2 arg2)
    {
    }

    private void FixedUpdate()
    {
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
        }

        if (FireOn)
        {
            if (lastShootT > shootTime)
            {
                lastShootT -= shootTime;

                points[0] = transform.position;
                points[1] = hit.point;
                lr.SetPositions(points);

            }
            else
            {
                lastShootT += Time.deltaTime;
            }

        }

    }

    public void OnShootBullet(InputAction.CallbackContext context)
    {
        Debug.Log("Testsdsdsdsd");
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                FireOn = true;
                lr.enabled = true;
                break;
            case InputActionPhase.Canceled:
                FireOn = false;
                lr.enabled = false;
                break;
            default:
                return;
        }
    }
}
