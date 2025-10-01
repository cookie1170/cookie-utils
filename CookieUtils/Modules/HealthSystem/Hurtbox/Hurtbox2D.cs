using System.Linq;
using UnityEngine;

namespace CookieUtils.HealthSystem
{
    /// <summary>
    /// A 2D implementation of a Hurtbox
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Hurtbox2D : Hurtbox
    {       
        /// <summary>
        /// Whether the trigger collider should be overriden explicitly<br/>
        /// If set to false, GetComponent searching for Collider2D is called
        /// </summary>
        public bool overrideTrigger;
        /// <summary>
        /// The trigger collider this Hurtbox is bound to
        /// </summary>
        public Collider2D trigger;

        protected override void Awake()
        {
            base.Awake();
            if (!overrideTrigger) trigger = GetComponent<Collider2D>();
            if (!trigger) throw new UnityException("Hurtbox2D needs a trigger");
            trigger.isTrigger = true;
            trigger.excludeLayers = int.MaxValue - LayerMask.GetMask("Hitboxes");
        }

        protected override (bool hitWall, Vector3 hitPoint) WallCheck(Vector3 position)
        {
            int mask = LayerMask.GetMask("Hitboxes") | WallMask;
            var results = new RaycastHit2D[8];
            var filter = new ContactFilter2D {
                useLayerMask = true,
                useTriggers = true,
                layerMask = mask
            };
            float distance = Vector3.Distance(transform.position, position);
            trigger.Raycast((position - transform.position).normalized, filter, results, distance);

            var resultsList = results.Where(result => result.transform && !IsSameGameObject(result.transform))
                .ToList();

            if (resultsList.Count == 0) return (false, transform.position);

            int wallIndex = resultsList
                .FindIndex(hit => ((1 << hit.transform.gameObject.layer) & WallMask) != 0);

            int hitboxIndex = resultsList
                .FindIndex(hit => hit.transform.gameObject.layer == HitboxesLayer);
            
            bool isWall = wallIndex != -1;
            var hitPoint = resultsList[hitboxIndex != -1 ? hitboxIndex : 0].point;

            return (isWall, hitPoint);
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            // could probably be optimized but i don't care
            if (!other.TryGetComponent(out Hitbox hitbox)) return;
            
            if (health.CheckMask(hitbox.GetInfo().Mask))
                HitboxesInRange.Add(hitbox);
        }
        protected void OnTriggerExit2D(Collider2D other)
        {
            // could probably be optimized but i don't care
            if (other.TryGetComponent(out Hitbox hitbox)) {
                HitboxesInRange.Remove(hitbox);
            }
        }
    }
}