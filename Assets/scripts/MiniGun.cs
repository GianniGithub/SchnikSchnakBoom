using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGun : MonoBehaviour
{
    public AnimationCurve ExpolisonCurv;
    public Transform PrefapExplosion;
    public PlayersControlls Controlls;
    LineRenderer lr;
    Vector3[] points;
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

    }

    private void MainShoot_performed(InputAction.CallbackContext context)
    {
        FireOn = true;
        lr.enabled = true;
    }

    private void MainShoot_canceled(InputAction.CallbackContext context)
    {
        FireOn = false;
        lr.enabled = false;

        points[0] = Vector3.zero;
        points[1] = Vector3.zero;
        lr.SetPositions(points);
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
                Projectile.CreateExplosion(hit.point, ExpolisonCurv, PrefapExplosion, 0.5f);

            }
            else
            {
                lastShootT += Time.deltaTime;
            }

        }

    }
    private void OnEnable()
    {

        Controlls.ControllEvents.Player1.MainShoot.performed += MainShoot_performed;
        Controlls.ControllEvents.Player1.MainShoot.canceled += MainShoot_canceled;
    }
    private void OnDisable()
    {

        Controlls.ControllEvents.Player1.MainShoot.performed -= MainShoot_performed;
        Controlls.ControllEvents.Player1.MainShoot.canceled -= MainShoot_canceled;
    }
}
