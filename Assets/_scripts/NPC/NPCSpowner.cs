using Gianni.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class NPCSpowner : MonoBehaviour
    {
        public GameObject PlayerPrefap;
        public Vector3 StartArena;
        public int Amount = 2;
        void Start()
        {
            GameEvents.Instance.StartListening(GameActions.OnPlayerAdded, OnPlayerAdded);
        }
        private void OnPlayerAdded(MonoBehaviour sender, GameEventArgs e)
        {
            for (int i = 0; i < Amount; i++)
            {
                SpownNPC();
            }
        }

        private void SpownNPC()
        {
            Vector3 spownPoint = NPCBehaivour.GetRandomNavPoint(20f, StartArena);
            var nPCobj = Instantiate(PlayerPrefap, spownPoint, Quaternion.identity);
            var pe = NPCEvents.AddNPC(NPCtype.dummy, nPCobj);

            var NPCe = new SpawnNPCArgs(NPCtype.dummy, nPCobj, pe);
            GameEvents.Instance.TriggerEvent(this, new GameEventArgs(GameActions.OnNPCAdded, NPCe));
        }
    }
    public class SpawnNPCArgs : EventArgs
    {
        public NPCtype Id { get; }
        public GameObject NPCobj { get; }
        public NPCEvents Pe { get; }
        public SpawnNPCArgs(NPCtype id, GameObject playerObj, NPCEvents pe)
        {
            Id = id;
            NPCobj = playerObj;
            Pe = pe;
        }
    }
}
