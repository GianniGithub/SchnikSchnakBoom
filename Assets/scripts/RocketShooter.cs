using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

namespace GellosGames
{
    public class RocketShooter : MonoBehaviour
    {
        public PlayersControlls Controlls;
        public Vector2 CrossRadiusRange;
        public Transform aimCrossPrefap;
        public Transform shootPrefap;
        Transform aimCross;
        private float range;

        void Start()
        {

        }
        void Update()
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.forward * range + transform.position, out hit, 4f, NavMesh.AllAreas))
            {
                aimCross.transform.position = hit.position;
            }
        }
        private void OnEnable()
        {
            Controlls.OnLookStateSwitch += Controlls_OnLookStateSwitch;
            Controlls.ControllEvents.Player1.MainShoot.performed += OnShootBullet;
            if (aimCross == null)
                aimCross = Instantiate(aimCrossPrefap);
            aimCross.gameObject.SetActive(true);

        }
        private void OnDisable()
        {
            Controlls.OnLookStateSwitch -= Controlls_OnLookStateSwitch;
            Controlls.ControllEvents.Player1.MainShoot.performed -= OnShootBullet;
            aimCross.gameObject.SetActive(false);
        }
        private void OnShootBullet(InputAction.CallbackContext obj)
        {
            if (!aimCross.gameObject.activeInHierarchy)
                return;

            var rocket = Instantiate(shootPrefap, transform.position, transform.rotation).GetComponent<Rocket>();
            rocket.aimCrossGoal = aimCross;

        }

        private void Controlls_OnLookStateSwitch(bool arg1, Vector2 axes)
        {
            var t = Mathf.Abs(axes.x) + Mathf.Abs(axes.y);
            range = Mathf.Lerp(CrossRadiusRange.x, CrossRadiusRange.y, t);
        }
    }

}