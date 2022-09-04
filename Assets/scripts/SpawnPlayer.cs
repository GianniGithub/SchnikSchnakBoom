using Gianni.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class SpawnPlayer : MonoBehaviour
    {
        public GameObject PlayerPrefap;
        public Vector3 Spownpoint;
        List<int> GampadIDsInUse = new List<int>();
        private void Awake()
        {
            PlayerEvents.Global = new PlayerEvents(PlayerID.all, gameObject);
        }
        void Start()
        {

            // Temp
            this.InvokeWait(10f, () => enabled = false);
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
                    var lalal = Instantiate(PlayerPrefap, Spownpoint, Quaternion.identity);
                    GampadIDsInUse.Add(Gamepad.all[i].deviceId);
                    PlayerEvents.AddPlayer((PlayerID)i, lalal);
                }
            }

        }

        private void OnDestroy()
        {

        }
        private void OnEnable()
        {

        }
        private void OnDisable()
        {

        }
    }
}
