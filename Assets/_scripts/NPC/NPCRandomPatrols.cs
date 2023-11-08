using Gianni.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

namespace GellosGames
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCRandomPatrols : NPCEvent
    {
        NavMeshAgent agent;
        private Vector3 destination;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        public override void OnNPCSpawn()
        {
            EventHandler.StartListening(NPCEventTrigger.OnNPCKilled, OnKilled);

            destination = GetRandomNavPoint(10f, transform.position);
            agent.destination = destination;
        }

        private void OnKilled(MonoBehaviour arg0, NPCEventArgs arg1)
        {
            throw new NotImplementedException();
        }

        public static Vector3 GetRandomNavPoint(float walkRadius, Vector3 startArena)
        {
            Vector3 randomDirection = (UnityEngine.Random.insideUnitCircle * walkRadius).ToVectorXZ();

            randomDirection += startArena;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            return hit.position;
        }
        void Update()
        {
            
            if (Vector3.Distance(destination, transform.position) < 4.0f)
            {
                destination = GetRandomNavPoint(30f, transform.position);
                agent.destination = destination;
            }
        }
    }
}
