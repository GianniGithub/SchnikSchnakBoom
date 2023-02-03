using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gianni.Helper;
using DG.Tweening;

namespace GellosGames
{
    public class Sword : Weapon
    {
        [SerializeField]
        AnimationCurve swingCurve;
        [SerializeField]
        Transform ChildAxe;
        [SerializeField]
        float swingDuration;
        [SerializeField]
        float degreesLength;
        Quaternion StartRotPosition;
        float t = 0f;
        TrailRenderer tr;
        Blade blade;

        private void Awake()
        {
            blade = GetComponentInChildren<Blade>();
            tr = GetComponentInChildren<TrailRenderer>();
            tr.time = swingDuration;
            tr.enabled = false;
        }
        public override void OnSpawn()
        {
            WeaponType = WeaponType.Sword;
            EventHandler.StartListening(PlayerActions.WeapenSwitch, onWeapenSwitch);
            StartRotPosition = ChildAxe.localRotation;

#if UNITY_EDITOR
            if (swingDuration > FireRate)
                Debug.LogError("swingDuration need to be smoaler then Firerate");
#endif
        }


        private void onWeapenSwitch(MonoBehaviour sender, PlayerEventArgs arg1)
        {
            if (((WeaponEvents)sender).Type == WeaponType.Sword)
            {
                EventHandler.ControlEvents.Player1.MainShoot.performed += MainShoot_performed;
                gameObject.SetActive(true);
            }
            else
            {
                EventHandler.ControlEvents.Player1.MainShoot.performed -= MainShoot_performed;
                gameObject.SetActive(false);
            }
        }

        private void MainShoot_performed(InputAction.CallbackContext obj)
        {
            if (IsFireTimeReady)
            {
                StartCoroutine(swingBlade());
            }
        }
        private IEnumerator swingBlade()
        {
            blade.enabled = true;
            float deltaDegree = degreesLength / swingDuration;
            tr.enabled = true;
            while (t < swingDuration)
            {
                t += Time.deltaTime;
                var deltaCurve = swingCurve.Evaluate(1 / swingDuration * t);
                ChildAxe.Rotate(0f, deltaDegree * Time.deltaTime * deltaCurve, 0f, Space.Self);
                yield return null; 
            }
            t = 0f;
            tr.enabled = false;
            blade.enabled = false;
            ChildAxe.localRotation = StartRotPosition;
        }

    }
    public class HitArgs : DamageArgs
    {
        public HitArgs(PlayerEvents originator, Vector3 hitPoint, WeaponType weapenType) : base(originator, weapenType, hitPoint)
        {

        }
    }
}
