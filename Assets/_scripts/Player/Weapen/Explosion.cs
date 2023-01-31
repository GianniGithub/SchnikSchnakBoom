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
            boom.DOScale(eA.ExplosionSize, 0.6f).SetEase(eA.ExplosionAnimation);
            var exploMaterial = boom.GetComponent<Renderer>().material;
            Tween tween = exploMaterial.DOFade(100f, 0.6f).OnComplete(() => UnityEngine.Object.Destroy(boom.gameObject));

            Collider[] colliders = Physics.OverlapSphere(eA.HitPoint, 1.3f);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.attachedRigidbody;

                if (rb != null)
                {
                    float damageForce = 150f * eA.ExplosionSize;
                    float radius = 1.5f * eA.ExplosionSize;

                    if (rb.gameObject.tag == "Player")
                    {
                        var dis = Vector3.Distance(eA.HitPoint, rb.transform.position);

                        var events = PlayerEvents.GetPlayerEventsHandler(rb.gameObject);
                        eA.Damage = radius / dis;
                        events.TriggerEvent(null, new PlayerEventArgs(PlayerActions.OnDamage, eA));

                        // as more damage the player have as 3 x more damgeforce
                        damageForce += eA.PlayerPoints.Damage * 3;
                    }
                    rb.AddExplosionForce(damageForce, eA.HitPoint, radius, 0.2f);
                }

            }

            return tween;
        }
    }

    public class ExplosionArgs : System.EventArgs
    {

        public ExplosionArgs(PlayerID originator, Vector3 hitPoint, float explosionSize, WeaponType weapenType, Transform explosionPrefap, AnimationCurve explosion)
        {
            this.originator = originator;
            HitPoint = hitPoint;
            ExplosionSize = explosionSize;
            WeapenType = weapenType;
            ExplosionPrefap = explosionPrefap;
            ExplosionAnimation = explosion;
        }

        public CollectHitPoint PlayerPoints { get; set; }
        public float Damage { get; set; }
        public PlayerID originator { get; }
        public Vector3 HitPoint { get; }
        public float ExplosionSize { get; }
        public WeaponType WeapenType { get; }
        public Transform ExplosionPrefap { get; }
        public AnimationCurve ExplosionAnimation { get; }
    }
}
