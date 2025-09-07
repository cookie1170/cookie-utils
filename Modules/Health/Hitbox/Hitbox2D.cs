using System;
using UnityEngine;

namespace CookieUtils.Health
{
    /// <summary>
    /// A 2D implementation of a Hitbox
    /// </summary>
    public class Hitbox2D : Hitbox
    {
        /// <summary>
        /// Whether the trigger collider should be overriden explicitly<br/>
        /// If set to false, GetComponent searching for Collider2D is called
        /// </summary>
        public bool overrideTrigger = false;
        /// <summary>
        /// The trigger collider this Hitbox is bound to
        /// </summary>
        public Collider2D trigger;
        /// <summary>
        /// Whether the Rigidbody should be overriden explicitly<br/>
        /// If set to false, GetComponent searching for Rigidbody is called
        /// </summary>
        public bool overrideRigidbody;
        /// <summary>
        /// The Rigidbody this Hitbox uses for direction calculation
        /// </summary>
        public Rigidbody2D rb;

        protected override void Awake()
        {
            base.Awake();
            if (directionType == DirectionTypes.Rigidbody && !overrideRigidbody) rb = GetComponentInParent<Rigidbody2D>();
            if (!overrideTrigger) trigger = GetComponent<Collider2D>();
            trigger.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Hitboxes");
        }
        
        protected override Vector3 GetDirection()
        {
            switch (directionType) {
                case DirectionTypes.Rigidbody: {
                    return rb.linearVelocity.normalized;
                }

                case DirectionTypes.Transform: {
                    throw new ArgumentException("Transform direction is not implemented yet");
                }

                case DirectionTypes.Manual: {
                    return direction;
                }
                
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}