using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace GellosGames
{
    public class CollectHitPoint : PlayerEvent
    {
        public float TotalDamage;
        public override void OnSpawn()
        {
            EventHandler.StartListening(PlayerActions.OnDamage, OnDamage);
            EventHandler.StartListening(PlayerActions.OnKilled, OnKilled);
        }
        private void OnKilled(MonoBehaviour arg0, PlayerEventArgs arg1)
        {
            EventHandler.StopListening(PlayerActions.OnDamage, OnDamage);
        }

        private void OnDamage(MonoBehaviour sender, PlayerEventArgs e)
        {
            var eA = (DamageArgs)e.EventInfos;
            AddDamage(eA.NewDamage);
            eA.PlayerPoints = this;
        }
        public void AddDamage(float damage)
        {
            this.TotalDamage += damage;
            //(TextMeshProUGUI)DamageInfo.text = this.TotalDamage.ToString("N1");
        }

    }
    public abstract class DamageArgs : System.EventArgs
    {
        protected DamageArgs(PlayerEvents originator, WeaponType weapenType, Vector3 hitPoint, float hitDamage)
        {
            this.originator = originator;
            WeapenType = weapenType;
            HitPoint = hitPoint;
            NewDamage = hitDamage;
        }
        public Vector3 HitPoint { get; }
        public PlayerEvents originator { get; }
        public float NewDamage { get; set; }
        public CollectHitPoint PlayerPoints { get; set; }
        public WeaponType WeapenType { get; }
    }
}
