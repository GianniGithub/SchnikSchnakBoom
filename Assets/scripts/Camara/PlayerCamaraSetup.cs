using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public class PlayerCamaraSetup : MonoBehaviour
    {
        public Camera[] ScreenCamps;
        public CinemachineVirtualCamera[] PlayerCamps;
        void Start()
        {
            GameEvents.Instance.StartListening(GameActions.OnPlayerAdded, OnPlayerAdded);

        }

        private void OnPlayerAdded(MonoBehaviour sender, GameEventArgs e)
        {
            var player = (SpawnPlayerArgs)e.EventInfos;
            int playerID = (int)player.Id;
            var PlayerCamp = PlayerCamps[playerID];

            ScreenCamps[playerID].enabled = true;
            PlayerCamp.enabled = true;
            PlayerCamp.gameObject.layer = LayerMask.NameToLayer(player.Id.ToString());
            PlayerCamp.Follow = player.PlayerObj.transform;
            PlayerCamp.LookAt = player.PlayerObj.transform;

            SetUpSplittScreen(playerID);
            player.Pe.StartListening(PlayerActions.OnKilled, OnKilled);
        }

        // Update is called once per frame
        private void SetUpSplittScreen(int player)
        {
            if (PlayerEvents.PlayerCount > 1)
            {
                ScreenCamps[0].rect = new Rect(0f, 0f, 0.5f, 1f);
                ScreenCamps[1].rect = new Rect(0.5f, 0, 0.5f, 1);
            }
            else
            {
                ScreenCamps[player].rect = new Rect(0, 0, 1, 1);
            }
        }
        private void OnKilled(MonoBehaviour arg0, PlayerEventArgs e)
        {
            int player = (int)e.From;
            ScreenCamps[player].enabled = false;
            PlayerCamps[player].enabled = false;
            SetUpSplittScreen(player);
        }
    }
}
