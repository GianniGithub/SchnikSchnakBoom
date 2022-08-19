using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rocket : MonoBehaviour
{
    public Transform goal;
    public Transform explosionPrefap;
    public AnimationCurve explosion;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {

        agent.destination = goal.position;
    }

    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.layer == 6)
        {
            var hitPoint = collision.GetContact(0).point;
            Projectile.CreateExplosion(hitPoint, explosion, explosionPrefap, 2f);
            gameObject.SetActive(false);
        }

    }
}
