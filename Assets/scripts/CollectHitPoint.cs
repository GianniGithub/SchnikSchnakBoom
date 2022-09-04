using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GellosGames;

public class CollectHitPoint : PlayerEvent
{
    public TextMeshProUGUI DamageInfo;
    public float Damage;
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddDamage(float damage)
    {
        this.Damage += damage;
        DamageInfo.text = this.Damage.ToString("N1");
    }
}
