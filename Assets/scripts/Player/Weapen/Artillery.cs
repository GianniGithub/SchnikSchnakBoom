using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class Artillery : Weapon
    {
        public ArtilleryProjectile bulletPrefab;
        public float PointsDistanzToEacheuser => lengthLimit / resulution;
        [SerializeField]
        float lengthLimit;
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
                proBullet.ArtilleryPathData = new ArtilleryPathPointData(points, rotations, lengthLimit / resulution);
            }
        }
        // Update is called once per frame
        void Update()
        {
            var point = transform.position; // + new Vector3(0,2,0);
            points[0] = point;
            rotations[0] = Quaternion.LookRotation(transform.up);

            for (int i = 1; i < resulution; i++)
            {
                var t = (lengthLimit / resulution) * i;

                //Positions
                var powerStraight = transform.up * Range * t;
                var gravityLost = (Mathf.Pow(t, 2) * 9.8f) / 2;
                points[i] = new Vector3(powerStraight.x, powerStraight.y - gravityLost, powerStraight.z) + point;

                //Rotations
                var direction = points[i] - points[i - 1];
                rotations[i] = Quaternion.LookRotation(direction);
            }
            lr.SetPositions(points);
        }
        private void FixedUpdate()
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (Physics.Linecast(points[i], points[++i], out RaycastHit hitInfo))
                {
                    aimCross.position = hitInfo.point;
                    Debug.Log("hit fix count: " + points.Length);
                    return;
                }
            }
        }
        private void OnDestroy()
        {
            EventHandler.StopListening(PlayerActions.WeapenSwitch, onWeapenSwitch);
        }
    }
    public class ArtilleryPathPointData
    {
        public Vector3[] points;
        public Quaternion[] rotations;
        public float PointsDistanzToEacheuser;

        public ArtilleryPathPointData(Vector3[] points, Quaternion[] rotations, float pointsDistanzToEacheuser) 
        {
            this.points = (Vector3[])points.Clone();
            this.rotations = (Quaternion[])rotations.Clone(); ;
            PointsDistanzToEacheuser = pointsDistanzToEacheuser;
        }
    }
}
