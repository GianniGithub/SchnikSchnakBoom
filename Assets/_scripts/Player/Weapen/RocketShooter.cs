using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

namespace GellosGames
{
    public class RocketShooter : LongRangeWeapon
    {
        public Transform shootPrefap;
        [SerializeField]
        Vector2 acurateAimRange;
        Vector2 currentAimRange;
        public override void OnSpawn()
        {
            PlayerControllEvents = EventHandler.ControlEvents.Player1;

            WeaponType = WeaponType.Rocket;
            EventHandler.StartListening(PlayerActions.WeapenSwitch, OnWeapenSwitch);
            EventHandler.StartListening(PlayerActions.OnKilled, OnKilled);
            EventHandler.StartListening(PlayerActions.OnLookStateChange, OnLookStateChange);

            if (aimCross == null)
                aimCross = Instantiate(aimCrossPrefap);

            enabled = false;
            aimCross.gameObject.SetActive(false);
        }

        protected override void OnWeapenSwitch(MonoBehaviour sender, PlayerEventArgs e)
        {
            if (((WeaponEvents)sender).Type == WeaponType.Rocket)
            {
                PlayerControllEvents.MainShoot.performed += OnShootBullet;
                gameObject.SetActive(true);
                SetWeaponAimMode();
            }
            else if (gameObject.activeInHierarchy)
            {
                PlayerControllEvents.MainShoot.performed -= OnShootBullet;
                aimCross.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }

        void Update()
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(GetAimPosition(transform.parent), out hit, 4f, NavMesh.AllAreas))
            {
                aimCross.transform.position = hit.position;
            }
        }
        private void OnShootBullet(InputAction.CallbackContext obj)
        {
            if (!aimCross.gameObject.activeInHierarchy || !IsFireTimeReady)
                return;

            Rocket rocket = Instantiate(shootPrefap, transform.position, transform.rotation).GetComponent<Rocket>();
            rocket.aimCrossGoal = aimCross;

            CallShootEvent();
        }
        protected override void OnAimmodeChanged(AimMode aimMode)
        {
            base.OnAimmodeChanged(aimMode);

            switch (aimMode)
            {
                case AimMode.off:
                    currentAimRange = AimRange;
                    PlayerControllEvents.looking.performed -= OnLooking;
                    PlayerControllEvents.WeapenMode.performed -= OnWeapenModeAccurateStart;
                    PlayerControllEvents.WeapenMode.canceled -= OnWeapenModeAccurateCanceld;
                    break;

                case AimMode.ControllerStickDirection:
                    currentAimRange = AimRange;
                    PlayerControllEvents.looking.performed += OnLooking;
                    PlayerControllEvents.WeapenMode.performed += OnWeapenModeAccurateStart;
                    PlayerControllEvents.WeapenMode.canceled += OnWeapenModeAccurateCanceld;
                    break;

                case AimMode.ControllerStickControlled:
                    currentAimRange = acurateAimRange;
                    break;

            }
        }
        protected override Vector2 GetAimRange() => currentAimRange;
    }

}