using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

namespace GellosGames
{
    public class VirtualCamaraSetup : PlayerEvent
    {
        static Camera P1;
        static Camera P2;
        CinemachineCamera cvc;
        public override void OnSpawn()
        {
            if (P1 == null || P2 == null)
            {
                var cams = Camera.allCameras;
                if (cams[0].name.StartsWith("P1"))
                {
                    P1 = cams[0];
                    P2 = cams[1];
                }
                else
                {
                    P1 = cams[1];
                    P2 = cams[0];
                }
            }

            SetUpSplittScreen();

            cvc = GetComponent<CinemachineCamera>();
            EventHandler.StartListening(PlayerActions.OnKilled, OnKilled);
            gameObject.layer = LayerMask.NameToLayer(EventHandler.id.ToString());
        }

        private void SetUpSplittScreen()
        {
            if (PlayerEvents.PlayerCount > 1)
            {
                P1.enabled = true;
                P2.enabled = true;
                P1.rect = new Rect(0f, 0f, 0.5f, 1f);
                P2.rect = new Rect(0.5f, 0, 0.5f, 1);
            }
            else
            {
                switch (EventHandler.id)
                {
                    case PlayerID.P1:
                        P2.enabled = false;
                        P1.rect = new Rect(0, 0, 1, 1);
                        break;
                    case PlayerID.P2:
                        P1.enabled = false;
                        P2.rect = new Rect(0, 0, 1, 1);
                        break;
                }
            }
        }

        private void OnKilled(MonoBehaviour arg0, PlayerEventArgs arg1)
        {
            SetUpSplittScreen();
        }

    }
}
