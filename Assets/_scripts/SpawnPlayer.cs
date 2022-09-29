using Gianni.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class SpawnPlayer : GameEvent
    {
        public Transform Parent;
        public GameObject PlayerPrefap;
        public Vector3[] Spownpoints;
        List<InputDevice> GampadIDsInUse = new List<InputDevice>();
        private void Awake()
        {
            
        }
        void Start()
        {
            // Temp
            this.InvokeWait(500f, () => enabled = false);

        }

        private void SpownOnClick(object obj, InputActionChange change)
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                if (inputAction.id != Guid.Parse("7607c7b6-cd76-4816-beef-bd0341cfe950"))
                    return;

                if (GampadIDsInUse.Contains(inputAction.activeControl.device))
                    return;

                GampadIDsInUse.Add(inputAction.activeControl.device);
                int PlayerID = GampadIDsInUse.Count - 1;
                var playerObj = Instantiate(PlayerPrefap, Spownpoints[PlayerID], Quaternion.identity, Parent);
                var pe = PlayerEvents.AddPlayer((PlayerID)PlayerID, playerObj, inputAction.activeControl.device);

                var e = new SpawnPlayerArgs(GampadIDsInUse.Count, (PlayerID)PlayerID, playerObj, pe);
                GameEvents.Instance.TriggerEvent(this, new GameEventArgs(GameActions.OnPlayerAdded, e));
            }
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
            if (GampadIDsInUse.Contains(obj.control.device))
                return;
            GampadIDsInUse.Add(obj.control.device);
            int PlayerID = GampadIDsInUse.Count - 1;
            var playerObj = Instantiate(PlayerPrefap, Spownpoints[PlayerID], Quaternion.identity, Parent);
            var pe = PlayerEvents.AddPlayer((PlayerID)PlayerID, playerObj, obj.control.device);

            var e = new SpawnPlayerArgs(GampadIDsInUse.Count, (PlayerID)PlayerID, playerObj, pe);
            GameEvents.Instance.TriggerEvent(this, new GameEventArgs(GameActions.OnPlayerAdded, e));
        }
        private void OnDisable()
        {
            var e = new InputManagerArgs() { Enabled = false };
            //GameEvents.Instance.TriggerEvent(null, new GameEventArgs(GameActions.OnGamePadStateChange, e));

            InputSystem.onActionChange -= SpownOnClick;
        }
        private void OnEnable()
        {
            //GameEvents.Instance.StartListening(GameActions.OnGamePadStateChange, OnGamePadStateChange);

            InputSystem.onActionChange += SpownOnClick;
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
