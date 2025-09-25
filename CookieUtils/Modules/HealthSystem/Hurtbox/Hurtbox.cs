using System.Collections.Generic;
using UnityEngine;

namespace CookieUtils.HealthSystem
{
    /// <summary>
    /// Abstract class for representing a dimensionless hurtbox
    /// </summary>
    public abstract class Hurtbox : MonoBehaviour
    {
        /// <summary>
        /// Whether the Health component should be overriden explicitly<br/>
        /// If set to false, GetComponentInParent searching for Health is called
        /// </summary>
        public bool overrideHealth;
        /// <summary>
        /// The Health component this Hurtbox is bound to
        /// </summary>
        public Health health;

        protected readonly List<Hitbox> HitboxesInRange = new();

        protected virtual void Awake()
        {
            if (!overrideHealth) health = GetComponentInParent<Health>();
        }

        protected void FixedUpdate()
        {
            for (int i = HitboxesInRange.Count - 1; i >= 0; i--) {
                if (i >= HitboxesInRange.Count) return; // avoid weird stuff with destroying
                var hitbox = HitboxesInRange[i];
                OnHit(hitbox);
            }
        }

        /// <summary>
        /// Should be called when a Hitbox is detected
        /// </summary>
        /// <param name="hitbox">The detected Hitbox</param>
        protected virtual void OnHit(Hitbox hitbox)
        {
            if (hitbox.data.hasPierce && hitbox.pierceLeft <= 0) return;
            
            if (health.TryGetHit(hitbox.GetInstanceID(), hitbox.GetInfo()))
                hitbox.OnAttack();
        }
    }
}
