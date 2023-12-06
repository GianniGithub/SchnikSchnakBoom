using Gianni.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class NPCSpowner : MonoBehaviour
    {
        public GameObject NPCPrefap;
        public Vector3 StartArena;
        public int Amount = 2;
        protected void Start()
        {
            //GameEvents.Instance.StartListening(GameActions.OnPlayerAdded, OnPlayerAdded);
        }
        private void OnPlayerAdded(MonoBehaviour sender, GameEventArgs e)
        {
            for (int i = 0; i < Amount; i++)
            {
                SpownNPC();
            }
        }
       
        [Button("SpownNPC")]
        private void SpownNPC()
        {
            Vector3 spownPoint = NPCRandomPatrols.GetRandomNavPoint(20f, StartArena);
            spownPoint.y = StartArena.y;
            var nPCobj = Instantiate(NPCPrefap, spownPoint, Quaternion.identity);
            var pe = NPCEvents.AddNPC(null, nPCobj);

            var NPCe = new SpawnNPCArgs(null, nPCobj, pe);
            GameEvents.Instance.TriggerEvent(this, new GameEventArgs(GameActions.OnNPCAdded, NPCe));
        }

    }
    public class SpawnNPCArgs : EventArgs
    {
        public NPCType Id { get; }
        public GameObject NPCobj { get; }
        public NPCEvents Pe { get; }
        public SpawnNPCArgs(NPCType id, GameObject playerObj, NPCEvents pe)
        {
            Id = id;
            NPCobj = playerObj;
            Pe = pe;
        }
    }
    public abstract class NPCType {}
}
