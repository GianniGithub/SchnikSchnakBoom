using System.Collections;
using System.Collections.Generic;
using Gianni.Helper;
using UnityEngine;
using NaughtyAttributes;

namespace GellosGames
{
    public class ShootTestBullet : MonoBehaviour
    {
        public Transform shootPrefap;
        public List<Transform> aimCrosses;
        public int ShootNr = 0;
        [Range(0f,2f)]
        public float TimeScale;
        // Start is called before the first frame update
        void Start()
        {
            this.InvokeWait(0.2f, OnShootBullet);
            //this.InvokeWait(5f, OnShootBullet);
        }

        // Update is called once per frame
        void Update()
        {
            Time.timeScale = TimeScale;
        }
        [Button("shoot specific Rockets")]
        private void ShootSpecificBullet() => ShootSpecificBullet(ShootNr);
        private void ShootSpecificBullet(int index)
        {
            if (!aimCrosses[index].gameObject.activeInHierarchy)
                return;

            Rocket rocket = Instantiate(shootPrefap, transform.position, transform.rotation).GetComponent<Rocket>();
            rocket.aimCrossTarget = aimCrosses[index];
        }
        [Button("shoot Rockets")]
        private void OnShootBullet()
        {
            for (int i = 0; i < aimCrosses.Count; i++)
            {
                ShootSpecificBullet(i);
            }
        }
        [Button("shoot Rockets loop")]
        private void OnShootBulletLoop()
        {
            this.InvokeWaitLoop(0.6f, ()=>ShootSpecificBullet(ShootNr));
        }

    }
}
