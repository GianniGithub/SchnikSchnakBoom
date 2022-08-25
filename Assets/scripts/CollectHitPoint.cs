using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectHitPoint : MonoBehaviour
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
