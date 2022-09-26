using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Gianni.Helper;

namespace GellosGames
{
    public class Artillery : Weapon
    {
        public ArtilleryProjectile bulletPrefab;
        [SerializeField]
        float BowSize = 1f;
        [SerializeField]
        int resulution;

        Transform aimCross;
        LineRenderer lr;
        Vector3[] points;
        Quaternion[] rotations;

        void Awake()
        {
            lr = GetComponent<LineRenderer>();
            lr.positionCount = resulution;
            points = new Vector3[resulution];
            rotations = new Quaternion[resulution];
        }
        public override void OnSpawn(InputDevice device) 
        {
            enabled = false;
            EventHandler.StartListening(PlayerActions.WeapenSwitch, onWeapenSwitch);

           
            if (aimCross == null)
                aimCross = Instantiate(aimCrossPrefap);
        }
        private void onWeapenSwitch(MonoBehaviour sender, PlayerEventArgs e)
        {
            PlayersControlls controlls = (PlayersControlls)sender;
            if (e.Current == Weapen.Artillery)
            {
                gameObject.SetActive(true);
                EventHandler.StartListening(PlayerActions.IsAiming, onAiming);
                SetEnableState(e.IsAiming, controlls);
            }
            else
            {
                if(enabled)
                    SetEnableState(false, controlls);
                gameObject.SetActive(false);
                EventHandler.StopListening(PlayerActions.IsAiming, onAiming);
            }
            
        }
        private void onAiming(MonoBehaviour sender, PlayerEventArgs e)
        {
            PlayersControlls controlls = (PlayersControlls)sender;
            SetEnableState(e.IsAiming, controlls);

        }
        void SetEnableState(bool onActive, PlayersControlls controlls)
        {
            if (onActive)
            {
                enabled = true;
                lr.enabled = true;
                aimCross.gameObject.SetActive(true);
                controlls.ControllEvents.Player1.looking.performed += OnLooking;
                controlls.ControllEvents.Player1.MainShoot.performed += OnShootBullet;
            }
            else
            {
                enabled = false;
                lr.enabled = false;
                aimCross.gameObject.SetActive(false);
                controlls.ControllEvents.Player1.looking.performed -= OnLooking;
                controlls.ControllEvents.Player1.MainShoot.performed -= OnShootBullet;
            }
        }

        public void OnShootBullet(InputAction.CallbackContext context)
        {
            if (IsFireTimeReady)
            {
                var proBullet = Instantiate(bulletPrefab);
                proBullet.OwnerId = EventHandler.id;
                proBullet.ArtilleryPathData = new ArtilleryPathPointData(points, rotations);
            }
        }
        // Update is called once per frame
        void Update()
        {
            var point = transform.position;
            points[0] = point;
            rotations[0] = Quaternion.LookRotation(transform.up);

            var grafAim = aimCross.position + new Vector3(0f, GravitationsFunktion(BowSize), 0f);
            var distanz = Vector3.Distance(grafAim, point);
            var ShootDirection = (grafAim - transform.position).normalized;

            //Debug.DrawLine(point, ShootDirection * distanz + point, Color.blue);

            var fx = ShootDirection * distanz / (resulution - 1);
            for (int i = 1; i < resulution; i++)
            {
                //Positions 
                Vector3 powerStraight = fx * i;
                float power = 1f / (resulution -1) * i;
                float gravityLost = GravitationsFunktion(power * BowSize);
                points[i] = new Vector3(powerStraight.x, powerStraight.y - gravityLost, powerStraight.z) + point;

                //Debug.DrawLine(powerStraight + point, points[i], Color.red);

                //Rotations
                var direction = points[i] - points[i - 1];
                rotations[i] = Quaternion.LookRotation(direction);
            }
            lr.SetPositions(points);
            transform.rotation = Quaternion.LookRotation(ShootDirection, Vector3.right);
        }

        private static float GravitationsFunktion(float fx)
        {
            return Mathf.Pow(fx, 2) * 9.8f / 2f;
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < points.Length - 1; i++)
            {
                if (Physics.Linecast(points[i], points[i + 1], out RaycastHit hitInfo))
                {
                    //aimCross.position = hitInfo.point;
                    return;
                }
            }
        }
        private void OnDestroy()
        {
            EventHandler.StopListening(PlayerActions.WeapenSwitch, onWeapenSwitch);
        }
    }
    public struct ArtilleryPathPointData
    {
        public Vector3[] points;
        public Quaternion[] rotations;

        public ArtilleryPathPointData(Vector3[] points, Quaternion[] rotations) 
        {
            this.points = (Vector3[])points.Clone();
            this.rotations = (Quaternion[])rotations.Clone(); ;
        }
    }
}
