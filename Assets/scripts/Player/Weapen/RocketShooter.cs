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

        public override void OnSpawn(UnityEngine.InputSystem.InputDevice device)
        {
            WeaponType = WeaponType.Rocket;
            EventHandler.StartListening(PlayerActions.WeapenSwitch, OnWeapenSwitch);
            EventHandler.StartListening(PlayerActions.OnKilled, OnKilled);

            if (aimCross == null)
                aimCross = Instantiate(aimCrossPrefap);

            enabled = false;
            aimCross.gameObject.SetActive(false);
        }

        protected override void OnWeapenSwitch(MonoBehaviour sender, PlayerEventArgs e)
        {
            PlayerControllEvents = ((PlayersControlls)sender).ControllEvents.Player1;

            if (e.Current == WeaponType.Rocket)
            {
                EventHandler.StartListening(PlayerActions.OnLookStateChange, OnLookStateChange);
                PlayerControllEvents.MainShoot.performed += OnShootBullet;
                gameObject.SetActive(true);
                OnLookStateChange(sender, e);
            }
            else if (gameObject.activeInHierarchy)
            {
                EventHandler.StopListening(PlayerActions.OnLookStateChange, OnLookStateChange);
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

        }
        protected override void OnAimmodeChanged(AimMode aimMode)
        {
            switch (aimMode)
            {
                case AimMode.off:
                    PlayerControllEvents.looking.performed -= OnLooking;
                    PlayerControllEvents.WeapenMode.performed -= OnWeapenModeAccurateStart;
                    PlayerControllEvents.WeapenMode.canceled -= OnWeapenModeAccurateCanceld;
                    break;

                case AimMode.ControllerStickDirection:
                    PlayerControllEvents.looking.performed += OnLooking;
                    PlayerControllEvents.WeapenMode.performed += OnWeapenModeAccurateStart;
                    PlayerControllEvents.WeapenMode.canceled += OnWeapenModeAccurateCanceld;
                    break;
            }
        }

    }

}