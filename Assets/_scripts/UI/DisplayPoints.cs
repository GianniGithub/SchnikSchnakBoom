using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace GellosGames
{
    public class DisplayPoints : MonoBehaviour
    {
        public TextMeshProUGUI DamageInfoPrefap;

        private void Awake()
        {
            GameEvents.Instance.StartListening(GameActions.OnPlayerAdded, OnPlayerAdded);
        }
        private void OnPlayerAdded(MonoBehaviour sender, GameEventArgs e)
        {
            var TextGui = Instantiate(DamageInfoPrefap, transform);
            var args = (SpawnPlayerArgs)e.EventInfos;

            var point = args.PlayerObj.GetComponent<CollectHitPoint>();
            point.DamageInfo = TextGui;

            var mesh = args.PlayerObj.GetComponent<MeshRenderer>();
            TextGui.color = mesh.material.color;

            args.Pe.StartListening(PlayerActions.OnKilled, (s,e)=> Destroy(gameObject));
        }

    }
}
