using Gianni.Helper;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class SpawnPlayer : GameEvent
    {
        public Transform Parent;
        public GameObject PlayerPrefap;
        public Vector3[] Spownpoints;
        Dictionary<InputDevice,PlayerDeviceConnection> GampadIDsInUse = new Dictionary<InputDevice, PlayerDeviceConnection>();
    
        void Start()
        {
            // Temp
            this.InvokeWait(500f, () => enabled = false);
            GameEvents.Instance.StartListening(GameActions.OnPlayerDied, OnPlayerDied);
        }

        private void OnPlayerDied(MonoBehaviour sender, GameEventArgs e)
        {
            var deathArgs = (DeathZoneArgs)e.EventInfos;
            var devPlay = GampadIDsInUse[deathArgs.Pe.ControlEvents.devices.Value[0]];
            devPlay.PlayerHasControl = false;
        }
        [Button("Spawn Player")]
        void spawnOnTestButton()
        {
            var gamepad = InputSystem.AddDevice<Gamepad>();
            spawnPlayer(gamepad);
        }
        private void spawnOnClick(object obj, InputActionChange change)
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                
                // Keybord
                if(inputAction.id == Guid.Parse("7b549163-8aa2-475e-8a8f-a1e14dd2a5fd"))
                    spawnPlayer(inputAction.activeControl.device);
                
                // Controller
                if (inputAction.id != Guid.Parse("7607c7b6-cd76-4816-beef-bd0341cfe950"))
                    spawnPlayer(inputAction.activeControl.device);
            }
        }
        private void spawnPlayer(InputDevice gamepad)
        {
            int PlayerNr = -1;

            if (!GampadIDsInUse.TryGetValue(gamepad, out var devPlay))
            {
                PlayerNr = GampadIDsInUse.Count;
                GampadIDsInUse.Add(gamepad, new PlayerDeviceConnection(PlayerNr, gamepad));
            }
            else if (devPlay.PlayerHasControl)
            {
                return;
            }
            else
            {
                PlayerNr = devPlay.PlayerNr;
                devPlay.PlayerHasControl = true;
            }
            var playerObj = Instantiate(PlayerPrefap, Spownpoints[PlayerNr], Quaternion.identity, Parent);
            var pe = PlayerEvents.AddPlayer((PlayerID)PlayerNr, playerObj, gamepad);
            playerObj.gameObject.name = pe.Name;

            var e = new SpawnPlayerArgs(GampadIDsInUse.Count, (PlayerID)PlayerNr, playerObj, pe);
            GameEvents.Instance.TriggerEvent(this, new GameEventArgs(GameActions.OnPlayerAdded, e));
        }

        private void OnGamePadStateChange(MonoBehaviour sender, GameEventArgs e)
        {
            var ima = (InputManagerArgs)e.EventInfos;

            if(ima.Enabled)
                InputManager.AllController.Player1.Conform.performed += Conform_performedSpawnPlayer;
            else
                InputManager.AllController.Player1.Conform.performed -= Conform_performedSpawnPlayer;
        }

        private void Conform_performedSpawnPlayer(InputAction.CallbackContext obj)
        {
        }
        private void OnDisable()
        {
            InputSystem.onActionChange -= spawnOnClick;
        }
        private void OnEnable()
        {
            InputSystem.onActionChange += spawnOnClick;
        }
        class PlayerDeviceConnection
        {
            public PlayerDeviceConnection(int playerID, InputDevice device)
            {
                PlayerNr = playerID;
                Device = device;
                PlayerHasControl = true;
            }
            public InputDevice Device { get; }
            public PlayerID PlayerID => (PlayerID)PlayerNr;
            public int PlayerNr { get; }
            public bool PlayerHasControl;
        }
    }



    public class SpawnPlayerArgs : EventArgs
    {
        public SpawnPlayerArgs(int count, PlayerID id, GameObject playerObj, PlayerEvents pe)
        {
            Count = count;
            Id = id;
            PlayerObj = playerObj;
            Pe = pe;
        }

        public int Count { get; }
        public PlayerID Id { get; }
        public GameObject PlayerObj { get; }
        public PlayerEvents Pe { get; }
    }
}
