using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace GellosGames
{
    public class CollectHitPoint : PlayerEvent
    {
        public TextMeshProUGUI DamageInfo;
        public float Damage;
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
            AddDamage(eA.Damage);
            eA.PlayerPoints = this;
        }
        public void AddDamage(float damage)
        {
            this.Damage += damage;
            DamageInfo.text = this.Damage.ToString("N1");
        }

    }
    public abstract class DamageArgs : System.EventArgs
    {
        protected DamageArgs(PlayerEvents originator, WeaponType weapenType, Vector3 hitPoint)
        {
            this.originator = originator;
            WeapenType = weapenType;
            HitPoint = hitPoint;
        }
        public Vector3 HitPoint { get; }
        public PlayerEvents originator { get; }
        public float Damage { get; set; }
        public CollectHitPoint PlayerPoints { get; set; }
        public WeaponType WeapenType { get; }
    }
}
