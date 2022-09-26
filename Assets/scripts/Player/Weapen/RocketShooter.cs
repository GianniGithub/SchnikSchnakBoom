using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

namespace GellosGames
{
    public class RocketShooter : Weapon
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

        private void onWeapenSwitch(MonoBehaviour sender, PlayerEventArgs e)
        {
            PlayersControlls controlls = (PlayersControlls)sender;
            if(e.Current == Weapen.Rocket)
            {
                EventHandler.StartListening(PlayerActions.OnAimStateChange, Aiming);
                controlls.ControllEvents.Player1.MainShoot.performed += OnShootBullet;
                gameObject.SetActive(true);
                Aiming(sender, e);
            }
            else if (gameObject.activeInHierarchy)
            {
                EventHandler.StopListening(PlayerActions.OnAimStateChange, Aiming);
                controlls.ControllEvents.Player1.MainShoot.performed -= OnShootBullet;
                aimCross.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }

        void Update()
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(GetAimPosition(transform), out hit, 4f, NavMesh.AllAreas))
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
        private void Aiming(MonoBehaviour sender, PlayerEventArgs e)
        {
            PlayersControlls controlls = sender as PlayersControlls;
            AimChangeBase(e.IsAiming, controlls, e.AimState);
            switch (e.AimState)
            {
                case AimMode.start:
                    controlls.ControllEvents.Player1.looking.performed += OnLooking;
                    break;
                case AimMode.accurate:
                    return;
                case AimMode.off:
                    controlls.ControllEvents.Player1.looking.performed -= OnLooking;
                    break;
            }
        }
        private void OnKilled(MonoBehaviour arg0, PlayerEventArgs arg1)
        {
            EventHandler.StopListening(PlayerActions.WeapenSwitch, onWeapenSwitch);
            Destroy(aimCross.gameObject);
        }
    }

}