using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Gianni.Helper;

public class Rocket : MonoBehaviour
{
    public Transform aimCrossGoal;
    public Transform explosionPrefap;
    public AnimationCurve explosion;
    Rigidbody rb;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {

        agent.destination = aimCrossGoal.position;

        if (Vector2.Distance(transform.position.ToVectorXZ(), aimCrossGoal.position.ToVectorXZ()) < 0.1f)
        {
            Projectile.CreateExplosion(transform.position, explosion, explosionPrefap, 2f);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb.detectCollisions = false;
        this.InvokeWait(0.5f, () => rb.detectCollisions = true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.layer == 6)
        {
            var hitPoint = collision.GetContact(0).point;
            Projectile.CreateExplosion(hitPoint, explosion, explosionPrefap, 2f);
            Destroy(gameObject);
        }
    }
}
