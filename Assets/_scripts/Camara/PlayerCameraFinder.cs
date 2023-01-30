using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GellosGames
{
    public class PlayerCameraFinder : PlayerEvent
    {
        [SerializeField]
        List<CameraDistance> DinstanceCol;
        public override void OnSpawn()
        {
            EventHandler.StartListening(PlayerActions.OnAimModeChange, OnAimModeChange);
        }

        private void OnAimModeChange(MonoBehaviour sender, PlayerEventArgs args)
        {
            LongRangeWeapon weapon = (LongRangeWeapon)sender;

            switch (weapon.AimModeState)
            {
                case AimMode.off:
                    transform.localPosition = new Vector3(0f, 0f, 0f);
                    break;
                case AimMode.ControllerStickDirection:
                    var dis = DinstanceCol.FirstOrDefault(x => x.weapenType == weapon.Type).distance;
                    transform.localPosition = dis != 0f ? new Vector3(0f, 0f, dis) : new Vector3(0f, 0f, 5f);
                    break;
                case AimMode.ControllerStickControlled:
                    transform.localPosition = new Vector3(0f, 0f, 8f);
                    break;
                default:
                    break;
            }
        }
    }
    [Serializable]
    public struct CameraDistance
    {
        public WeaponType weapenType;
        public float distance;
    }
}
