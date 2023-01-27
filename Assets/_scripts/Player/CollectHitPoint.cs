using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GellosGames;
using System;

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
        var eA = (ExplosionArgs)e.EventInfos;
        AddDamage(eA.Damage);
        eA.PlayerPoints = this;
    }
    public void AddDamage(float damage)
    {
        this.Damage += damage;
        DamageInfo.text = this.Damage.ToString("N1");
    }
}
