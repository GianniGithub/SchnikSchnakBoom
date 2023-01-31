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
        public CinemachineVirtualCamera[] PlayerAimCamps;

        PlayerCameraFinder[] PlayerPointer;
        void Start()
        {
            GameEvents.Instance.StartListening(GameActions.OnPlayerAdded, OnPlayerAdded);
            PlayerPointer = new PlayerCameraFinder[4];
        }

        private void OnPlayerAdded(MonoBehaviour sender, GameEventArgs e)
        {
            var player = (SpawnPlayerArgs)e.EventInfos;
            int playerID = (int)player.Id;
            var PlayerCamp = PlayerCamps[playerID];

            ScreenCamps[playerID].enabled = true;
            PlayerCamp.enabled = true;
            PlayerCamp.gameObject.layer = LayerMask.NameToLayer(player.Id.ToString());

            PlayerPointer[playerID] = player.PlayerObj.GetComponentInChildren<PlayerCameraFinder>();
            PlayerCamp.Follow = PlayerPointer[playerID].transform; //player.PlayerObj.transform;

            player.Pe.StartListening(PlayerActions.OnAimModeChange, OnAimModeChange);

            SetUpSplittScreen(playerID);
            player.Pe.StartListening(PlayerActions.OnKilled, OnKilled);
        }

        private void OnAimModeChange(MonoBehaviour sender, PlayerEventArgs e)
        {
            LongRangeWeapon weapon = (LongRangeWeapon)sender;
            int playerID = (int)e.From;

            switch (weapon.AimModeState)
            {
                case AimMode.off:
                    PlayerAimCamps[playerID].gameObject.SetActive(false);
                    break;
                case AimMode.ControllerStickDirection:
                    PlayerAimCamps[playerID].Follow = PlayerPointer[playerID].transform;
                    PlayerAimCamps[playerID].gameObject.SetActive(true);
                    break;
                case AimMode.ControllerStickControlled:
                    PlayerAimCamps[playerID].Follow = weapon.AimCross;
                    PlayerAimCamps[playerID].gameObject.SetActive(true);
                    break;
            }
            
        }
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
