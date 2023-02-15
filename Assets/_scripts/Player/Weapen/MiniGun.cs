using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class MiniGun : Weapon
    {
        [SerializeField]
        float damagePower = 150f;
        [SerializeField]
        AnimationCurve ExpolisonCurv;
        [SerializeField]
        Transform PrefapExplosion;

        LineRenderer lr;
        Vector3[] points;
        bool FireOn = false;

        RaycastHit hit;

        private void Awake()
        {
            lr = GetComponent<LineRenderer>();
        }
        public override void OnSpawn()
        {
            WeaponType = WeaponType.Gun;
            EventHandler.StartListening(PlayerActions.WeapenSwitch, onWeapenSwitch);
        }

        private void onWeapenSwitch(MonoBehaviour sender, PlayerEventArgs e)
        {
            if (((WeaponEvents)sender).Type == WeaponType.Gun)
            {
                EventHandler.ControlEvents.Player1.MainShoot.performed += MainShoot_performed;
                EventHandler.ControlEvents.Player1.MainShoot.canceled += MainShoot_canceled;
                gameObject.SetActive(true);
            }
            else
            {
                EventHandler.ControlEvents.Player1.MainShoot.performed -= MainShoot_performed;
                EventHandler.ControlEvents.Player1.MainShoot.canceled -= MainShoot_canceled;
                gameObject.SetActive(false);
            }
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
            Fire();
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
            Fire();
        }

        private void Fire()
        {
            if (FireOn && IsFireTimeReady)
            {
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);

                    points[0] = transform.position;
                    points[1] = hit.point;
                    lr.SetPositions(points);

                    // for explosion efect reasions, is only working if outside of the collider
                    var explosionPosition = hit.point - ((points[1] - points[0]).normalized * 0.1f);
                    var exp = new ExplosionArgs(EventHandler, explosionPosition, 0.8f, WeaponType.Gun, PrefapExplosion, ExpolisonCurv, damagePower);
                    Explosion.CreateExplosion(exp);
                }
                else
                {
                    points[0] = transform.position;
                    points[1] = transform.TransformDirection(Vector3.up) * 100f;
                    lr.SetPositions(points);
                }
            }

            CallShootEvent();
        }
    }

}