using UnityEngine;

namespace CookieUtils.Health
{
    /// <summary>
    /// A 3D implementation of a Hurtbox
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Hurtbox3D : Hurtbox
    {       
        /// <summary>
        /// Whether the trigger collider should be overriden explicitly<br/>
        /// If set to false, GetComponent searching for Collider is called
        /// </summary>
        public bool overrideTrigger;
        /// <summary>
        /// The trigger collider this Hurtbox is bound to
        /// </summary>
        public Collider trigger;

        protected override void Awake()
        {
            base.Awake();
            if (!overrideTrigger) trigger = GetComponent<Collider>();
            if (!trigger) throw new UnityException("Hurtbox3D needs a trigger");
            trigger.isTrigger = true;
            trigger.excludeLayers = int.MaxValue - LayerMask.GetMask("Hitboxes");
        }

        protected void OnTriggerEnter(Collider other)
        {
            // could probably be optimized but i don't care
            if (other.TryGetComponent(out Hitbox hitbox)) {
                OnHit(hitbox);
            }
        }
    }
}