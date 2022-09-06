using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Gianni.Helper;

namespace GellosGames
{
    public class ArtilleryProjectile : MonoBehaviour, Bullet
    {
        public Transform explosionPrefap;
        public AnimationCurve explosion;
        public ArtilleriePathPointData PathData;
        Rigidbody rb;
        float trackCounter = 0f;
        public float speed = 1f;
        int i = 0;

        public PlayerID OwnerId { get; set; }
        Transform Bullet.ExplosionPrefap => explosionPrefap;
        AnimationCurve Bullet.ExplosionAnimation => explosion;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

        }
        void Start()
        {
            rb.detectCollisions = false;
            this.InvokeWait(0.5f, () => rb.detectCollisions = true);

        }

        private void OnCollisionEnter(Collision collision)
        {
            var exp = new ExplosionArgs(OwnerId, transform.position, 2.7f, Weapen.Artillery, explosionPrefap, explosion);
            Explosion.CreateExplosion(exp);
            Destroy(gameObject);

        }
        void Update()
        {
            trackCounter += speed * Time.deltaTime;

            while (trackCounter > PathData.PointsDistanzToEacheuser)
            {
                trackCounter -= PathData.PointsDistanzToEacheuser;

                if (i + 2 >= PathData.points.Length)
                {
                    enabled = false;
                    return;
                }
                else
                {
                    i++;
                }
            }

            var t = trackCounter / PathData.PointsDistanzToEacheuser;

            transform.SetPositionAndRotation(Vector3.Lerp(PathData.points[i], PathData.points[i + 1], t), Quaternion.Lerp(PathData.rotations[i], PathData.rotations[i + 1], t));
        }

    }

}