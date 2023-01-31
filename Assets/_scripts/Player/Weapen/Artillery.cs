using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Gianni.Helper;

namespace GellosGames
{
    public class Artillery : LongRangeWeapon
    {
        public ArtilleryProjectile bulletPrefab;
        [SerializeField]
        float BowSize = 1f;
        [SerializeField]
        int resulution;

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
        public override void OnSpawn() 
        {
            PlayerControllEvents = EventHandler.ControlEvents.Player1;

            WeaponType = WeaponType.Artillery;
            enabled = false;
            EventHandler.StartListening(PlayerActions.WeapenSwitch, OnWeapenSwitch);
            EventHandler.StartListening(PlayerActions.OnKilled, OnKilled);
            EventHandler.StartListening(PlayerActions.OnLookStateChange, OnLookStateChange);

            if (aimCross == null)
                aimCross = Instantiate(aimCrossPrefap);
        }
        protected override void OnWeapenSwitch(MonoBehaviour sender, PlayerEventArgs e)
        {
            if (((WeaponEvents)sender).Type == WeaponType.Artillery)
            {
                gameObject.SetActive(true);
                EventHandler.StartListening(PlayerActions.OnLookStateChange, OnLookStateChange);
                SetWeaponAimMode();
            }
            else
            {
                if(enabled)
                    OnLookStateChange(null, e);
                gameObject.SetActive(false);
                EventHandler.StopListening(PlayerActions.OnLookStateChange, OnLookStateChange);
            }
            
        }
        protected override void OnAimmodeChanged(AimMode aimMode)
        {
            base.OnAimmodeChanged(aimMode);
            switch (aimMode)
            {
                case AimMode.off:
                    lr.enabled = false;
                    PlayerControllEvents.looking.performed -= OnLooking;
                    PlayerControllEvents.MainShoot.performed -= OnShootBullet;

                    PlayerControllEvents.WeapenMode.performed -= OnWeapenModeAccurateStart;
                    PlayerControllEvents.WeapenMode.canceled -= OnWeapenModeAccurateCanceld;
                    return;

                case AimMode.ControllerStickDirection:
                    lr.enabled = false;
                    PlayerControllEvents.looking.performed += OnLooking;
                    PlayerControllEvents.WeapenMode.performed += OnWeapenModeAccurateStart;
                    PlayerControllEvents.WeapenMode.canceled += OnWeapenModeAccurateCanceld;
                    CheckIfWeapenModeAlreadyPerformed();
                    return;

                case AimMode.ControllerStickControlled:
                    lr.enabled = true;
                    PlayerControllEvents.MainShoot.performed += OnShootBullet;
                    return;
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

            CallShootEvent();
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

        private static float GravitationsFunktion(float delta)
        {
            return Mathf.Pow(delta, 2) * 9.8f / 2f;
        }

        private void FixedUpdate()
        {
            var aim = GetAimPosition(transform.parent);
            //Debug.DrawRay(aim + new Vector3(0f, 7f, 0f), Vector3.down, Color.red);
            if (Physics.Raycast(aim + new Vector3(0f, 7f, 0f), Vector3.down, out RaycastHit hitInfo, 30f))
            {
                aimCross.position = hitInfo.point;
                return;
            }
            else
                aimCross.position = aim;

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
