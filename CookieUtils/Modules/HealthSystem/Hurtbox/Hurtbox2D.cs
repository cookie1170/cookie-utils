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

        protected void OnTriggerEnter2D(Collider2D other)
        {
            // could probably be optimized but i don't care
            if (other.TryGetComponent(out Hitbox hitbox)) {
                HitboxesInRange.Add(hitbox);
            }
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