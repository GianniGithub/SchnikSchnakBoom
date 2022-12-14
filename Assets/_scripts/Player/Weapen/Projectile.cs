using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Gianni.Helper;

namespace GellosGames
{
    public abstract class Projectile : MonoBehaviour, Bullet
    {
        public Transform explosionPrefap;
        public AnimationCurve explosion;

        public float speed = 1f;
        protected Rigidbody rb;


        protected bool alreadyHit = false;


        public PlayerID OwnerId { get; set; }
        Transform Bullet.ExplosionPrefap => explosionPrefap;
        AnimationCurve Bullet.ExplosionAnimation => explosion;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
    }

}
