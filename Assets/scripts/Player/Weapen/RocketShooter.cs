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
            EventHandler.StartListening(PlayerActions.WeapenSwitch, onWeapenSwitch);
            EventHandler.StartListening(PlayerActions.OnKilled, OnKilled);

            if (aimCross == null)
                aimCross = Instantiate(aimCrossPrefap);

            enabled = false;
            aimCross.gameObject.SetActive(false);
        }

        protected override void onWeapenSwitch(MonoBehaviour sender, PlayerEventArgs e)
        {
            PlayerControllEvents = ((PlayersControlls)sender).ControllEvents.Player1;

            if (e.Current == Weapen.Rocket)
            {
                EventHandler.StartListening(PlayerActions.OnAimStateChange, OnAimStateChange);
                PlayerControllEvents.MainShoot.performed += OnShootBullet;
                gameObject.SetActive(true);
                OnAimStateChange(sender, e);
            }
            else if (gameObject.activeInHierarchy)
            {
                EventHandler.StopListening(PlayerActions.OnAimStateChange, OnAimStateChange);
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
        private void OnAimStateChange(MonoBehaviour sender, PlayerEventArgs e)
        {
            AimChangeBase(e.IsAiming, e.AimState);

            switch (e.AimState)
            {
                case AimMode.off:
                    PlayerControllEvents.looking.performed -= OnLooking;
                    PlayerControllEvents.WeapenMode.performed -= OnWeapenModeAccurateStart;
                    PlayerControllEvents.WeapenMode.canceled -= OnWeapenModeAccurateCanceld;
                    break;

                case AimMode.start:
                    PlayerControllEvents.looking.performed += OnLooking;
                    PlayerControllEvents.WeapenMode.performed += OnWeapenModeAccurateStart;
                    PlayerControllEvents.WeapenMode.canceled += OnWeapenModeAccurateCanceld;
                    break;

                case AimMode.accurate:
                    return;
            }
        }

    }

}