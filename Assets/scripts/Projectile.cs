using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    public Transform explosionPrefap;
    public AnimationCurve explosion;
    void Start()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            var hitPoint = collision.GetContact(0).point;
            var boom = Instantiate(explosionPrefap, hitPoint, Quaternion.identity);
            boom.DOScale(2, 0.6f).SetEase(explosion);
            var exploMaterial = boom.GetComponent<Renderer>().material;
            exploMaterial.DOFade(100f, 0.6f).OnComplete(()=>Destroy(boom.gameObject));

            Collider[] colliders = Physics.OverlapSphere(hitPoint, 1);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(5f, hitPoint, 1.5f, 3.0F);
            }
            gameObject.SetActive(false);
        }

    }
}
