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

        }
        void Start()
        {

            // Temp
            this.InvokeWait(10f, () => enabled = false);
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var pad in Gamepad.all)
            {
                if (GampadIDsInUse.Contains(pad.deviceId))
                    continue;

                if (pad.aButton.isPressed)
                {
                    var lalal = Instantiate(PlayerPrefap, Spownpoint, Quaternion.identity);
                    GampadIDsInUse.Add(pad.deviceId);
                    PlayerEvents.AddPlayer((PlayerID)GampadIDsInUse.Count, pad.deviceId, lalal);
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
