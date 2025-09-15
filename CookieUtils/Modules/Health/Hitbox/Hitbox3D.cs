using System;
using UnityEngine;

namespace CookieUtils.Health
{
    /// <summary>
    /// A 3D implementation of a Hitbox
    /// </summary>
    public class Hitbox3D : Hitbox
    {
        [Tooltip("Whether the trigger collider should be overriden explicitly\n If set to false, GetComponent searching for Collider is called")]
        public bool overrideTrigger = false;

        [Tooltip("The trigger collider this Hitbox is bound to")]
        public Collider trigger;

        [Tooltip("Whether the Rigidbody should be overriden explicitly\n If set to false, GetComponentInParent searching for Rigidbody is called")]
        public bool overrideRigidbody;

        [Tooltip("The Rigidbody this Hitbox uses for direction calculation")]
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