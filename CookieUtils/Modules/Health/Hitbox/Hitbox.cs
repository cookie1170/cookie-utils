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

        [Tooltip("Whether to use an AttackData ScriptableObject")]
        public bool useDataObject = false;

        [Tooltip("An AttackData scriptable object used for the data of this object")]
        public AttackData data;

        [Tooltip("The mask used for hit detection\n Set to int.MaxValue to pass all checks")]
        public int mask;

        [Tooltip("The damage dealt by the Hitbox")]
        public int damage = 20;

        [Range(0.05f, 3f), Tooltip("The I-Frames invoked by the Hitbox")]
        public float iframes = 0.2f;

        [Tooltip("Whether the Hitbox has a limited pierce")]
        public bool hasPierce = false;

        [Range(1, 20), Tooltip("The amount of pierce the Hitbox has until it can no longer attack\n Only used if hasPierce is true")]
        public int pierce = 1;

        [Tooltip("Whether to destroy the object when pierce runs out")]
        public bool destroyOnOutOfPierce = true;

        [Tooltip("Whether to destroy the parent GameObject")]
        public bool destroyParent = true;

        [Tooltip("The delay to destroy the GameObject when pierce runs out")]
        public float destroyDelay;

        [Tooltip("The direction type used by the hitbox")]
        public DirectionTypes directionType;

        [Tooltip("The direction override for when the Manual direction type is used"), NonSerialized]
        public Vector3 direction;

        [Tooltip("The remaining pierce of the Hitbox")]
        public int pierceLeft;

        [Tooltip("Invoked when the Hitbox\'s pierce runs out")]
        public UnityEvent onOutOfPierce;

        [Tooltip("Invoked when the Hitbox attacks something")]
        public UnityEvent onAttack;

        #endregion

        protected virtual void Awake()
        {
            if (useDataObject && data) {
                UpdateData();
            }

            pierceLeft = pierce;
        }

        public void UpdateData()
        {
            mask = data.mask;
            damage = data.damage;
            iframes = data.iframes;
            hasPierce = data.hasPierce;
            pierce = data.pierce;
            destroyOnOutOfPierce = data.destroyOnOutOfPierce;
            destroyParent = data.destroyParent;
            destroyDelay = data.destroyDelay;
            directionType = data.directionType;
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
