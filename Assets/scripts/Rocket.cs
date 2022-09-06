using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Gianni.Helper;

namespace GellosGames
{
    public class Rocket : MonoBehaviour, Bullet
    {
        public Transform aimCrossGoal;
        public Transform explosionPrefap;
        public AnimationCurve explosion;
        Rigidbody rb;
        NavMeshAgent agent;

        public PlayerID OwnerId { get; set; }
        Transform Bullet.ExplosionPrefap => explosionPrefap;
        AnimationCurve Bullet.ExplosionAnimation => explosion;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
        }
        void Update()
        {
            agent.destination = aimCrossGoal.position;

            if (Vector2.Distance(transform.position.ToVectorXZ(), aimCrossGoal.position.ToVectorXZ()) < 0.1f)
            {
                TriggerExplosion();
            }
        }

        void Start()
        {
            rb.detectCollisions = false;
            this.InvokeWait(0.5f, () => rb.detectCollisions = true);
        }

        private void OnCollisionEnter(Collision collision)
        {
            TriggerExplosion();
        }

        private void TriggerExplosion()
        {
            var exp = new ExplosionArgs(OwnerId, transform.position, 2f, Weapen.Rocket, explosionPrefap, explosion);
            Explosion.CreateExplosion(exp);
            Destroy(gameObject);
        }
    } 
}
