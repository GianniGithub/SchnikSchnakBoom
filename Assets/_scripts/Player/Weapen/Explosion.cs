using UnityEngine;
using DG.Tweening;
using Gianni.Helper;

namespace GellosGames
{
    public static class Explosion
    {
        public static Tween CreateExplosion(ExplosionArgs eA) // Alles in EventArgs und eigene Explosionsbeahvior
        {
            var boom = UnityEngine.Object.Instantiate(eA.ExplosionPrefap, eA.HitPoint, Quaternion.identity);
            boom.DOScale(eA.ExplosionRadius, 0.6f).SetEase(eA.ExplosionAnimation);
            var exploMaterial = boom.GetComponent<Renderer>().material;
            Tween tween = exploMaterial.DOFade(100f, 0.6f).OnComplete(() => UnityEngine.Object.Destroy(boom.gameObject));

            Collider[] colliders = Physics.OverlapSphere(eA.HitPoint, eA.ExplosionRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.attachedRigidbody;
                if (rb != null)
                {
                    Vector3 closestPoint = eA.HitPoint;
                    var damageForce = 100f;
                    if (hit.gameObject.tag == "Player")
                    {
                        closestPoint = rb.ClosestPointOnBounds(eA.HitPoint);
                        float distance = Vector3.Distance(closestPoint, eA.HitPoint);
                        eA.NewDamage *= 1.0F - Mathf.Clamp01(distance / eA.ExplosionRadius);

                        var events = PlayerEvents.GetPlayerEventsHandler(rb.gameObject);
                        events.TriggerEvent(null, new PlayerEventArgs(PlayerActions.OnDamage, eA));

                        Debug.Log("New Explo Damage: " + eA.NewDamage);
                        // as more damage the player have as 3 x more damgeforce
                        damageForce += eA.PlayerPoints.TotalDamage;
                    }
                    rb.AddExplosionForce(damageForce, closestPoint, eA.ExplosionRadius, 0.2f);
                }

            }

            return tween;
        }
    }

    public class ExplosionArgs : DamageArgs
    {
        public ExplosionArgs(PlayerEvents originator, Vector3 hitPoint, float explosionSize, WeaponType weapenType, Transform explosionPrefap, AnimationCurve explosion, float hitDamage) : base(originator, weapenType, hitPoint, hitDamage)
        {
            ExplosionRadius = explosionSize;
            ExplosionPrefap = explosionPrefap;
            ExplosionAnimation = explosion;
        }
        public float ExplosionRadius { get; }
        public Transform ExplosionPrefap { get; }
        public AnimationCurve ExplosionAnimation { get; }
    }


}
