using Unity.Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public class PlayerCamaraSetup : MonoBehaviour
    {
        public Camera[] ScreenCamps;
        public CinemachineCamera[] PlayerCamps;
        public CinemachineCamera[] PlayerAimCamps;


        private PlayerCameraFinder[] playerPointer;
        private CinemachineBrain[] brains;
        void Start()
        {
            GameEvents.Instance.StartListening(GameActions.OnPlayerAdded, OnPlayerAdded);
            playerPointer = new PlayerCameraFinder[4];
            brains = new CinemachineBrain[ScreenCamps.Length];
            for (int i = 0; i < ScreenCamps.Length; i++)
            {
                brains[i] = ScreenCamps[i].GetComponent<CinemachineBrain>();
            }
        }

        private void OnPlayerAdded(MonoBehaviour sender, GameEventArgs e)
        {
            var player = (SpawnPlayerArgs)e.EventInfos;
            int playerID = (int)player.Id;
            var PlayerCamp = PlayerCamps[playerID];
            var layer = LayerMask.NameToLayer(player.Id.ToString());

            ScreenCamps[playerID].enabled = true;
            PlayerCamp.gameObject.layer = layer;
            PlayerCamp.gameObject.SetActive(true);
            PlayerCamp.Priority = 0;
            //CinemachineBrain.SoloCamera = PlayerCamp;

            PlayerAimCamps[playerID].gameObject.layer = layer;
            playerPointer[playerID] = player.PlayerObj.GetComponentInChildren<PlayerCameraFinder>();
            PlayerCamp.Follow = playerPointer[playerID].transform; //player.PlayerObj.transform;
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
                    PlayerAimCamps[playerID].Follow = playerPointer[playerID].transform;
                    PlayerAimCamps[playerID].gameObject.SetActive(true);
                    PlayerAimCamps[playerID].Lens.OrthographicSize = 10f;
                    break;
                case AimMode.ControllerStickControlled:
                    PlayerAimCamps[playerID].Follow = weapon.AimCross;
                    PlayerAimCamps[playerID].gameObject.SetActive(true);
                    PlayerAimCamps[playerID].Lens.OrthographicSize = 17f;
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
