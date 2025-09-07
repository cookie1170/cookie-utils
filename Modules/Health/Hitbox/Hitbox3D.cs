using System;
using UnityEngine;

namespace CookieUtils.Health
{
    /// <summary>
    /// A 3D implementation of a Hitbox
    /// </summary>
    public class Hitbox3D : Hitbox
    {
        /// <summary>
        /// Whether the trigger collider should be overriden explicitly<br/>
        /// If set to false, GetComponent searching for Collider is called
        /// </summary>
        public bool overrideTrigger = false;
        /// <summary>
        /// The trigger collider this Hitbox is bound to
        /// </summary>
        public Collider trigger;
        /// <summary>
        /// Whether the Rigidbody should be overriden explicitly<br/>
        /// If set to false, GetComponent searching for Rigidbody is called
        /// </summary>
        public bool overrideRigidbody;
        /// <summary>
        /// The Rigidbody this Hitbox uses for direction calculation
        /// </summary>
        public Rigidbody rb;

        protected override void Awake()
        {
            base.Awake();
            if (directionType == DirectionTypes.Rigidbody && !overrideRigidbody) rb = GetComponentInParent<Rigidbody>();
            if (!overrideTrigger) trigger = GetComponent<Collider>();
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