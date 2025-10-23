using System;
using CookieUtils.Debugging;
using UnityEngine;
using UnityEngine.Events;

namespace CookieUtils.HealthSystem
{
    /// <summary>
    ///     An abstract class representing a dimensionless hitbox
    /// </summary>
    public abstract class Hitbox : MonoBehaviour, IDebugDrawer
    {
        /// <summary>
        ///     Ways of getting the attack's direction
        /// </summary>
        public enum DirectionType
        {
            /// <summary>
            ///     Calculates the direction from the Transform
            /// </summary>
            Transform,

            /// <summary>
            ///     Manually set using direction
            /// </summary>
            Manual,
        }

        [Tooltip("An AttackData scriptable object used for the data of this object")]
        public AttackData data;

        [Tooltip("The remaining pierce of the Hitbox")]
        public int pierceLeft;

        [Tooltip("Invoked when the Hitbox\'s pierce runs out")]
        public UnityEvent onOutOfPierce;

        [Tooltip("Invoked when the Hitbox attacks something")]
        public UnityEvent onAttack;

        [Tooltip("The direction override for when the Manual direction type is used")] [NonSerialized]
        public Vector3 direction;

        protected virtual void Awake() {
            if (!data) {
                Debug.LogError($"{(transform.parent ? transform.parent.name : name)}'s Hitbox has no data object!");
                Destroy(this);

                return;
            }

            pierceLeft = data.pierce;
        }

        protected virtual void Start() {
            CookieDebug.Register(this);
        }

        public void DrawDebugUI(IDebugUIBuilderProvider provider) {
            provider.Get(transform.parent.gameObject ? transform.parent.gameObject : gameObject)
                .Foldout("Hitbox", "hitbox")
                .Label($"Damage: {data.damage}", "hitbox-damage")
                .Label($"Mask: {Convert.ToString(data.mask, 2)}", "hitbox-mask")
                .EndFoldout();
        }

        /// <summary>
        ///     Gets the HitboxInfo of this hitbox
        /// </summary>
        /// <returns>A HitboxInfo struct generated from this Hitbox's properties</returns>
        public virtual HitboxInfo GetInfo() => new(data.damage, data.iframes, GetDirection(), data.mask);

        /// <summary>
        ///     Called when the Hitbox attacks a Hurtbox, must pass check
        /// </summary>
        public virtual void OnAttack() {
            onAttack?.Invoke();

            if (!data.hasPierce) return;

            pierceLeft--;
            if (pierceLeft <= 0) {
                onOutOfPierce?.Invoke();

                if (!data.destroyOnOutOfPierce) return;

                Transform objToDestroy = data.destroyParent ? transform.parent : transform;
                objToDestroy ??= transform;
                Destroy(objToDestroy.gameObject, data.destroyDelay);
            }
        }

        /// <summary>
        ///     Abstract method to get the direction of the attack
        /// </summary>
        /// <returns>The direction of the attack</returns>
        protected abstract Vector3 GetDirection();

        /// <summary>
        ///     Struct used for hit information
        /// </summary>
        public struct HitboxInfo
        {
            /// <summary>
            ///     The damage the hit deals
            /// </summary>
            public readonly int Damage;

            /// <summary>
            ///     Direction of the hit
            /// </summary>
            public readonly Vector3 Direction;

            /// <summary>
            ///     How long to apply invincibility for (in seconds)
            /// </summary>
            public readonly float Iframes;

            /// <summary>
            ///     The bitwise mask used for hit comparison<br />
            ///     Set to int.MaxValue to pass any test
            /// </summary>
            public readonly int Mask;

            public HitboxInfo(int damage, float iframes, Vector3 direction, int mask = int.MaxValue) {
                Damage = damage;
                Iframes = iframes;
                Mask = mask;
                Direction = direction;
            }
        }
    }
}