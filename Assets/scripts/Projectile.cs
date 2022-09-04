using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Gianni.Helper;
using GellosGames;

public class Projectile : MonoBehaviour
{
    public Transform explosionPrefap;
    public AnimationCurve explosion;
    private Rigidbody rb;

    public PlayerID Owner { get; internal set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rb.detectCollisions = false;
        this.InvokeWait(0.5f, () => rb.detectCollisions = true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CreateExplosion(transform.position, explosion, explosionPrefap, 2.7f);
        Destroy(gameObject);

    }

    public static Tween CreateExplosion(EventArgs info, Vector3 hitPoint, AnimationCurve explosion, Transform explosionPrefap, float explosionSize) // Alles in EventArgs und eigene Explosionsbeahvior
    {
        var boom = Instantiate(explosionPrefap, hitPoint, Quaternion.identity);
        boom.DOScale(explosionSize, 0.6f).SetEase(explosion);
        var exploMaterial = boom.GetComponent<Renderer>().material;
        Tween tween = exploMaterial.DOFade(100f, 0.6f).OnComplete(()=>Destroy(boom.gameObject));

        Collider[] colliders = Physics.OverlapSphere(hitPoint, 1.3f);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.attachedRigidbody;

            if (rb != null)
            {
                float addDamage = 150f * explosionSize;
                float radius = 1.5f * explosionSize;
                if ( rb.gameObject.tag == "Player")
                {
                    
                    var damage = rb.gameObject.GetComponent<CollectHitPoint>();
                    var dis = Vector3.Distance(hitPoint, rb.transform.position);

                    var events = PlayerEvents.GetPlayerEventsHandler(rb.gameObject);
                    events.TriggerEvent(rb, info);
                    damage.AddDamage(radius / dis);
                    addDamage += damage.Damage * 3;
                }
                rb.AddExplosionForce(addDamage, hitPoint, radius);
            }
             
        }

        return tween;
    }
}
