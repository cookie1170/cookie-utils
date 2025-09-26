using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace CookieUtils.HealthSystem
{
    /// <summary>
    /// A MonoBehaviour class used for tracking health and death
    /// </summary>
    [PublicAPI]
    public class Health : MonoBehaviour
    {
        #region Serialized fields

        [Tooltip("A HealthData scriptable object used for the data of this object")]
        public HealthData data;

        [field: Tooltip("The current amount of health. Use Hit or Regen to edit externally")]
        public float HealthAmount { get; protected set; } = 100;

        [Tooltip("The event invoked on hit, not invoked for fatal hits")]
        public UnityEvent<HitInfo> onHit;

        [Tooltip("The event invoked on death")]
        public UnityEvent<HitInfo> onDeath;

        [Tooltip("Is the object dead\nUse Kill to set it externally")]
        public bool IsDead { get; protected set; }

        #endregion

        /// <summary>
        /// Used for tracking local I-Frames
        /// </summary>
        protected readonly Dictionary<int, float> LocalIframes = new();

        /// <summary>
        /// Used for tracking global I-Frames
        /// </summary>
        protected float GlobalIframes;

        /// <summary>
        /// The time since the object last got hit, used for tracking regeneration
        /// </summary>
        public float TimeSinceHit { get; protected set; }

        protected virtual void Awake()
        {
            if (!data) {
                Debug.LogError($"{name}'s Health has no data object!");
                Destroy(this);
                return;
            }

            HealthAmount = data.startHealth;
        }

        protected virtual void Update()
        {
            if (data.hasRegen) HandleRegen();

            switch (data.iframeType) {
                case IframeTypes.Local: {
                    int[] keys = LocalIframes.Keys.ToArray();

                    foreach (int hitboxId in keys) {
                        LocalIframes[hitboxId] -= Time.deltaTime;
                    }

                    for (int i = LocalIframes.Keys.Count - 1; i >= 0; i--) {
                        int key = keys[i];
                        float time = LocalIframes[key];

                        if (time <= 0) {
                            LocalIframes.Remove(key);
                        }
                    }

                    break;
                }

                case IframeTypes.Global: {
                    GlobalIframes -= Time.deltaTime;
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Used to regenerate the object's health by amount
        /// </summary>
        /// <param name="amount">Amount to regenerate the health by</param>
        public virtual void Regen(float amount)
        {
            HealthAmount += amount;
            HealthAmount = Mathf.Clamp(HealthAmount, 0, data.maxHealth);
        }

        /// <summary>
        /// Used to deal damage to the object
        /// </summary>
        /// <param name="info">The HitInfo used for the hit</param>
        public virtual void Hit(HitInfo info)
        {
            if (IsDead) return;

            TimeSinceHit = 0f;

            HealthAmount -= info.Damage;
            if (HealthAmount <= 0) {
                Kill(info);
                return;
            }

            onHit?.Invoke(info);
        }

        /// <summary>
        /// Used to kill the object, can be called prematurely
        /// </summary>
        /// <param name="info">The HitInfo used for the death</param>
        public virtual void Kill(HitInfo info)
        {
            if (IsDead) return;

            IsDead = true;
            HealthAmount = 0;
            onDeath?.Invoke(info);

            if (data.destroyOnDeath) {
                Destroy(gameObject, data.destroyDelay);
            }
        }

        /// <summary>
        /// Handles the passive heath regeneration, only called if hasRegen is true
        /// </summary>
        protected virtual void HandleRegen()
        {
            TimeSinceHit += Time.deltaTime;
            float healAmount = data.regenCurve.Evaluate(TimeSinceHit) * Time.deltaTime;
            Regen(healAmount);
        }

        /// <summary>
        /// Called by a Hurtbox when it detects a Hitbox
        /// </summary>
        /// <param name="instanceId">The instance ID of the Hitbox</param>
        /// <param name="info">The HitInfo of the Hitbox</param>
        /// <returns>Whether the hit check passed or not</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when iframeType is out of range</exception>
        public virtual bool TryGetHit(int instanceId, HitInfo info)
        {
            if ((data.mask & info.Mask) == 0) {
                return false;
            }

            switch (data.iframeType) {
                case IframeTypes.Local: {
                    if (CheckLocalIframes(instanceId)) 
                    {
                        LocalIframes[instanceId] = info.Iframes * data.iframeMult;
                        Hit(info);
                        return true;
                    }

                    break;
                }
                case IframeTypes.Global: {
                    if (GlobalIframes <= 0) {
                        GlobalIframes = info.Iframes * data.iframeMult;
                        Hit(info);
                        return true;
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }

        public bool CheckLocalIframes(int instanceId)
        {
            float iframes = LocalIframes.GetValueOrDefault(instanceId, 0f);
            return iframes <= 0;
        }

#if UNITY_EDITOR
        protected virtual void OnGUI()
        {
            if (!CookieDebug.IsDebugMode) return;

            string maskBinary = Convert.ToString(data.mask, 2);
            CookieDebug.DrawLabelWorld($"Health: {HealthAmount:N0}/{data.maxHealth}", transform.position + Vector3.up);
            CookieDebug.DrawLabelWorld($"Hurt Mask: {maskBinary}", transform.position + Vector3.up * 1.5f);
        }
#endif

        /// <summary>
        /// Struct used for hit information
        /// </summary>
        public struct HitInfo
        {
            /// <summary>
            /// The damage the hit deals
            /// </summary>
            public readonly int Damage;

            /// <summary>
            /// Direction of the hit
            /// </summary>
            public readonly Vector3 Direction;

            /// <summary>
            /// How long to apply invincibility for (in seconds)
            /// </summary>
            public readonly float Iframes;

            /// <summary>
            /// The bitwise mask used for hit comparison<br/>
            /// Set to int.MaxValue to pass any test
            /// </summary>
            public readonly int Mask;

            public HitInfo(int damage, float iframes, Vector3 direction, int mask = int.MaxValue)
            {
                Damage = damage;
                Iframes = iframes;
                Mask = mask;
                Direction = direction;
            }
        }

        /// <summary>
        /// The types of I-Frames that can be used by a hurtbox
        /// </summary>
        public enum IframeTypes
        {
            /// <summary>
            /// I-Frames based on the hitbox, each hitbox has its own I-Frames value
            /// </summary>
            Local,

            /// <summary>
            /// I-Frames based on the hurtbox, while they're active no hitbox can damage the hurtbox
            /// </summary>
            Global,
        }
    }
}
