using System;
using UnityEngine;
using UnityEngine.Events;

namespace CookieUtils.Health
{
    /// <summary>
    /// An abstract class representing a dimensionless hitbox 
    /// </summary>
    public abstract class Hitbox : MonoBehaviour
    {
        #region Serialized fields
        /// <summary>
        /// Whether to use an AttackData ScriptableObject
        /// </summary>
        public bool useDataObject = false;
        /// <summary>
        /// An AttackData scriptable object used for the data of this object
        /// </summary>
        public AttackData data;
        /// <summary>
        /// The mask used for hit detection<br/>
        /// Set to int.MaxValue to pass all checks
        /// </summary>
        public int mask;
        /// <summary>
        /// The damage dealt by the Hitbox
        /// </summary>
        public int damage = 20;
        /// <summary>
        /// The I-Frames invoked by the Hitbox
        /// </summary>
        public float iframes = 0.2f;
        /// <summary>
        /// Whether the Hitbox has a limited pierce 
        /// </summary>
        public bool hasPierce = false;
        /// <summary>
        /// The amount of pierce the Hitbox has until it can no longer attack<br/>
        /// Only used if hasPierce is true
        /// </summary>
        public int pierce = 1;
        /// <summary>
        /// Whether to destroy the object when pierce runs out
        /// </summary>
        public bool destroyOnOutOfPierce = true;
        /// <summary>
        /// Whether to destroy the parent GameObject
        /// </summary>
        public bool destroyParent = true;
        /// <summary>
        /// The delay to destroy the GameObject when pierce runs out
        /// </summary>
        public float destroyDelay;
        /// <summary>
        /// The direction type used by the hitbox
        /// </summary>
        public DirectionTypes directionType;
        /// <summary>
        /// The direction override for when the Manual direction type is used
        /// </summary>
        public Vector3 direction;
        /// <summary>
        /// The remaining pierce of the Hitbox
        /// </summary>
        public int pierceLeft;
        /// <summary>
        /// Invoked when the Hitbox's pierce runs out
        /// </summary>
        public UnityEvent onOutOfPierce;
        /// <summary>
        /// Invoked when the Hitbox attacks something
        /// </summary>
        public UnityEvent onAttack;    
        #endregion
        
        protected virtual void Awake()
        {
            if (useDataObject && data) {
                mask = data.mask;
                damage = data.damage;
                iframes = data.iframes;
                hasPierce = data.hasPierce;
                pierce = data.pierce;
            }

            pierceLeft = pierce;
        }

        /// <summary>
        /// Gets the HitInfo of this hitbox
        /// </summary>
        /// <returns>A HitInfo struct generated from this Hitbox's properties</returns>
        public virtual Health.HitInfo GetInfo()
        {
            return new Health.HitInfo(damage, iframes, GetDirection(), mask);
        }
        
        /// <summary>
        /// Called when the Hitbox attacks a Hurtbox, must pass check
        /// </summary>
        public virtual void OnAttack()
        {
            onAttack?.Invoke();
            if (!hasPierce) return;
            
            pierceLeft--;
            if (pierceLeft <= 0) {
                onOutOfPierce?.Invoke();
                if (destroyOnOutOfPierce) {
                    var objToDestroy = destroyParent ? transform.parent : transform;
                    objToDestroy ??= transform;
                    Destroy(objToDestroy.gameObject, destroyDelay);
                }
            }
        }

        /// <summary>
        /// Abstract method to get the direction of the attack
        /// </summary>
        /// <returns>The direction of the attack</returns>
        protected abstract Vector3 GetDirection();
        
#if UNITY_EDITOR
        protected virtual void OnGUI()
        {
            if (!CookieDebug.IsDebugMode) return;

            string maskBinary = Convert.ToString(mask, 2);
            CookieDebug.DrawLabelWorld($"Attack Mask: {maskBinary}", transform.position + Vector3.down * 1.5f);
            if (hasPierce) {
                CookieDebug.DrawLabelWorld($"Pierce: {pierceLeft}/{pierce}", transform.position + Vector3.down * 2);
            }
        }
#endif

        /// <summary>
        /// Ways of getting the attack's direction
        /// </summary>
        public enum DirectionTypes
        {
            /// <summary>
            /// Gets the direction from an attached Rigidbody
            /// </summary>
            Rigidbody,
            /// <summary>
            /// Calculates the direction from the Transform
            /// </summary>
            Transform,
            /// <summary>
            /// Manually set using direction
            /// </summary>
            Manual,
        }
    }
}
