using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class MiniGun : PlayerEvent, Bullet
    {
        public AnimationCurve ExpolisonCurv;
        public Transform PrefapExplosion;

        public float shootTime;
        LineRenderer lr;
        Vector3[] points;
        bool FireOn = false;
        float lastShootT;
        RaycastHit hit;

        public PlayerID OwnerId { get; set; }
        public Transform ExplosionPrefap => PrefapExplosion;
        public AnimationCurve ExplosionAnimation => ExpolisonCurv;

        private void Awake()
        {
            lr = GetComponent<LineRenderer>();
        }
        public override void OnSpawn()
        {
            EventHandler.StartListening(PlayerActions.WeapenSwitch, onWeapenSwitch);
            OwnerId = EventHandler.id;
        }

        private void onWeapenSwitch(MonoBehaviour sender, PlayerEventArgs e)
        {
            PlayersControlls controlls = (PlayersControlls)sender;
            if (e.Current == Weapen.Gun)
            {
                controlls.ControllEvents.Player1.MainShoot.performed += MainShoot_performed;
                controlls.ControllEvents.Player1.MainShoot.canceled += MainShoot_canceled;
                gameObject.SetActive(true);
            }
            else
            {
                controlls.ControllEvents.Player1.MainShoot.performed -= MainShoot_performed;
                controlls.ControllEvents.Player1.MainShoot.canceled -= MainShoot_canceled;
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
            lastShootT = shootTime;
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
            if (FireOn)
            {
                if (lastShootT >= shootTime)
                {
                    lastShootT -= shootTime;

                    // Does the ray intersect any objects excluding the player layer
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);

                        points[0] = transform.position;
                        points[1] = hit.point;
                        lr.SetPositions(points);

                        // for explosion efect reasions, is only working if outside of the collider
                        var explosionPosition = hit.point - ((points[1] - points[0]).normalized * 0.1f);
                        var exp = new ExplosionArgs(OwnerId, explosionPosition, 0.8f, Weapen.Gun, PrefapExplosion, ExpolisonCurv);
                        Explosion.CreateExplosion(exp);
                    }
                    else
                    {
                        points[0] = transform.position;
                        points[1] = transform.TransformDirection(Vector3.up) * 100f;
                        lr.SetPositions(points);
                    }
                }
                else
                {
                    lastShootT += Time.deltaTime;
                }
            }
        }
        private void OnDestroy()
        {
            EventHandler.StopListening(PlayerActions.WeapenSwitch, onWeapenSwitch);
        }
    }

}