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
        public GameObject PlayerPrefap;
        public Vector3 Spownpoint;
        List<int> GampadIDsInUse = new List<int>();
        void Start()
        {

            // Temp
            this.InvokeWait(100f, () => enabled = false);
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < Gamepad.all.Count; i++)
            {
                if (GampadIDsInUse.Contains(Gamepad.all[i].deviceId))
                    continue;

                if (Gamepad.all[i].aButton.isPressed)
                {
                    var playerObj = Instantiate(PlayerPrefap, Spownpoint, Quaternion.identity);
                    GampadIDsInUse.Add(Gamepad.all[i].deviceId);
                    var pe = PlayerEvents.AddPlayer((PlayerID)i, playerObj);

                    var e = new SpawnPlayerArgs(GampadIDsInUse.Count, (PlayerID)i, playerObj, pe);
                    EventHandler.TriggerEvent(this, new GameEventArgs(GameActions.OnPlayerAdded, e));
                }
            }

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
