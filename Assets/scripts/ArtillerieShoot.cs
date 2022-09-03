using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GellosGames
{
    public class ArtillerieShoot : MonoBehaviour
    {
        public Projectile bulletPrefab;
        public PlayersControlls Controlls;
        Transform bullet;
        public float speed;
        [SerializeField]
        ArtilleriePathPointData pathData;
        ArtilleriePath aPath;
        private float trackCounter = 0f;
        bool lookLock;

        int i = 0;

        void Start()
        {
            enabled = false;
            aPath = GetComponent<ArtilleriePath>();
            pathData = aPath.GetArtilleriePathData();

        }

        public void Controlls_OnLookStateSwitch(bool arg1, Vector2 arg2) => lookLock = arg1;


        void Update()
        {
            trackCounter += speed * Time.deltaTime;

            while (trackCounter > pathData.PointsDistanzToEacheuser)
            {
                trackCounter -= pathData.PointsDistanzToEacheuser;

                if (i + 2 >= pathData.points.Length)
                {
                    enabled = false;
                    return;
                }
                else
                {
                    i++;
                }
            }

            var t = trackCounter / pathData.PointsDistanzToEacheuser;
            if (bullet == null)
                return;
            bullet.SetPositionAndRotation(Vector3.Lerp(pathData.points[i], pathData.points[i + 1], t), Quaternion.Lerp(pathData.rotations[i], pathData.rotations[i + 1], t));
        }
        public void OnShootBullet(InputAction.CallbackContext context)
        {
            Debug.Log("bbb");
            if (!lookLock || enabled)
                return;

            aPath = GetComponent<ArtilleriePath>();
            pathData = aPath.GetArtilleriePathData();
            bullet = Instantiate(bulletPrefab).transform;
            trackCounter = 0f;
            i = 0;
            pathData = aPath.GetArtilleriePathData();
            enabled = true;
        }

    }

}