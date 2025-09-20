using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CookieUtils.Health
{
    /// <summary>
    /// A MonoBehaviour class used for tracking health and death
    /// </summary>
    public class Health : MonoBehaviour
    {
        #region Serialized fields

        [Tooltip("Whether to use a HealthData ScriptableObject")]
        public bool useDataObject = false;

        [Tooltip("A HealthData scriptable object used for the data of this object")]
        public HealthData data;

        [Tooltip("The bitwise mask used for hit comparison\n Set to int.MaxValue to pass all checks")]
        public int mask;

        [Min(1), Tooltip("The maximum amount of health it can reach")]
        public int maxHealth = 100;

        [Min(1), Tooltip("The starting amount of health")]
        public int startHealth = 100;

        [Tooltip("Whether the object can regenerate health passively")]
        public bool hasRegen = false;

        [Tooltip("The curve used for passive health regeneration\n The curve\'s value at a certain time is the amount it regenerates per second")]
        public AnimationCurve regenCurve = AnimationCurve.EaseInOut(0, 0, 5, 5);

        [Tooltip("The type of I-Frames used\nLocal - per hitbox, \nGlobal - per hurtbox")]
        public IframeTypes iframeType = IframeTypes.Local;

        [Min(0.05f), Tooltip("The multiplier applied to the I-Frames")]
        public float iframeMult = 1f;
        
        [SerializeField, Tooltip("The current amount of health")]
        private float healthAmount = 100;
        
        [Tooltip("The current amount of health\nUse Hit or Regen to edit externally")]
        public float HealthAmount {
            get => healthAmount;
            protected set => healthAmount = value;
        }

        [Tooltip("Whether to destroy the GameObject on death")]
        public bool destroyOnDeath;

        [Tooltip("The delay for destroying the GameObject on death")]
        public float destroyDelay;

        [Tooltip("The event invoked on hit, not invoked for fatal hits")]
        public UnityEvent<HitInfo> onHit;

        [Tooltip("The event invoked on death")]
        public UnityEvent<HitInfo> onDeath;

        [field: SerializeField, Tooltip("Is the object dead\nUse Kill to set it externally")]
        public bool isDead { get; protected set; }

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
        public float timeSinceHit { get; protected set; }

        protected virtual void Awake()
        {
            if (useDataObject && data) {
                UpdateData();
            }

            HealthAmount = startHealth;
        }

        public void UpdateData()
        {
            mask = data.mask;
            maxHealth = data.maxHealth;
            startHealth = data.startHealth;
            hasRegen = data.hasRegen;
            regenCurve = data.regenCurve;
            iframeType = data.iframeType;
            iframeMult = data.iframeMult;
            destroyOnDeath = data.destroyOnDeath;
            destroyDelay = data.destroyDelay;
        }

        protected virtual void Update()
        {
            if (hasRegen) HandleRegen();

            switch (iframeType) {
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
            HealthAmount = Mathf.Clamp(HealthAmount, 0, maxHealth);
        }

        /// <summary>
        /// Used to deal damage to the object
        /// </summary>
        /// <param name="info">The HitInfo used for the hit</param>
        public virtual void Hit(HitInfo info)
        {
            if (isDead) return;

            timeSinceHit = 0f;

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
            if (isDead) return;

            isDead = true;
            HealthAmount = 0;
            onDeath?.Invoke(info);

            if (destroyOnDeath) {
                Destroy(gameObject, destroyDelay);
            }
        }

        /// <summary>
        /// Handles the passive heath regeneration, only called if hasRegen is true
        /// </summary>
        protected virtual void HandleRegen()
        {
            timeSinceHit += Time.deltaTime;
            float healAmount = regenCurve.Evaluate(timeSinceHit) * Time.deltaTime;
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
            if ((mask & info.Mask) == 0) {
                return false;
            }

            switch (iframeType) {
                case IframeTypes.Local: {
                    if (CheckLocalIframes(instanceId)) 
                    {
                        LocalIframes[instanceId] = info.Iframes * iframeMult;
                        Hit(info);
                        return true;
                    }

                    break;
                }
                case IframeTypes.Global: {
                    if (GlobalIframes <= 0) {
                        GlobalIframes = info.Iframes * iframeMult;
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

            string maskBinary = Convert.ToString(mask, 2);
            CookieDebug.DrawLabelWorld($"Health: {HealthAmount:N0}/{maxHealth}", transform.position + Vector3.up);
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
