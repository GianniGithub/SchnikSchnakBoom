using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Gianni.Helper;

namespace GellosGames
{
    public class ArtilleryProjectile : Projectile
    {
        public ArtilleryPathPointData ArtilleryPathData;
        float trackCounter = 0f;
        float distanz = 0f;
        int i = 0;
        void Start()
        {
            rb.detectCollisions = false;
            this.InvokeWait(0.1f, () => rb.detectCollisions = true);
            distanz = Vector3.Distance(ArtilleryPathData.points[i], ArtilleryPathData.points[i + 1]);

        }
        private void OnCollisionEnter(Collision collision)
        {
            DoExplosion(transform.position);
        }

        private void DoExplosion(Vector3 hitPoint)
        {
            if (!alreadyHit)
            {
                var exp = new ExplosionArgs(OwnerId, hitPoint, 2.7f, Weapen.Artillery, explosionPrefap, explosion);
                Explosion.CreateExplosion(exp);
                Destroy(gameObject);
                alreadyHit = true;
            }
        }

        void FixedUpdate()
        {
            trackCounter += speed * Time.fixedDeltaTime;

            while (trackCounter > distanz)
            {
                trackCounter -= distanz;
                if (i + 2 >= ArtilleryPathData.points.Length)
                {
                    var hitpoint = ArtilleryPathData.points[ArtilleryPathData.points.Length - 1];
                    DoExplosion(hitpoint);
                }
                else
                {
                    i++;
                    distanz = Vector3.Distance(ArtilleryPathData.points[i], ArtilleryPathData.points[i + 1]);
                }
            }
            MoveOnTrack();
        }

        private void MoveOnTrack()
        {
            var t = trackCounter / distanz;

            rb.MovePosition(Vector3.LerpUnclamped(ArtilleryPathData.points[i], ArtilleryPathData.points[i + 1], t));
            rb.MoveRotation(Quaternion.LerpUnclamped(ArtilleryPathData.rotations[i], ArtilleryPathData.rotations[i + 1], t));
        }
    }

}