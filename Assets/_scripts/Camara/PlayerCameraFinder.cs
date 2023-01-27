using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public class PlayerCameraFinder : PlayerEvent
    {
        public override void OnSpawn()
        {
            EventHandler.StartListening(PlayerActions.OnAimModeChange, OnAimModeChange);
        }

        private void OnAimModeChange(MonoBehaviour sender, PlayerEventArgs arg1)
        {
            LongRangeWeapon weapon = (LongRangeWeapon)sender;
            switch (weapon.AimModeState)
            {
                case AimMode.off:
                    transform.localPosition = new Vector3(0f, 0f, 0f);
                    break;
                case AimMode.ControllerStickDirection:
                    transform.localPosition = new Vector3(0f, 0f, 5f);
                    break;
                case AimMode.ControllerStickControlled:
                    transform.localPosition = new Vector3(0f, 0f, 8f);
                    break;
                default:
                    break;
            }
        }
    }
}
