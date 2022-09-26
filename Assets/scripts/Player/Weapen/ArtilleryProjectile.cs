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
            if (!alreadyHit)
            {
                var exp = new ExplosionArgs(OwnerId, transform.position, 2.7f, Weapen.Artillery, explosionPrefap, explosion);
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
                    enabled = false;
                    return;
                }
                else
                {
                    i++;
                    distanz = Vector3.Distance(ArtilleryPathData.points[i], ArtilleryPathData.points[i + 1]);
                }
            }

            var t = trackCounter / distanz;

            rb.MovePosition(Vector3.Lerp(ArtilleryPathData.points[i], ArtilleryPathData.points[i + 1], t));
            rb.MoveRotation(Quaternion.Lerp(ArtilleryPathData.rotations[i], ArtilleryPathData.rotations[i + 1], t));
        }

    }

}