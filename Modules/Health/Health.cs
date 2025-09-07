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

        /// <summary>
        /// Whether to use a HealthData ScriptableObject
        /// </summary>
        public bool useDataObject = false;

        /// <summary>
        /// A HealthData scriptable object used for the data of this object
        /// </summary>
        public HealthData data;

        /// <summary>
        /// The bitwise mask used for hit comparison<br/>
        /// Set to int.MaxValue to pass all checks
        /// </summary>
        public int mask;

        /// <summary>
        /// The maximum amount of health it can reach
        /// </summary>
        public int maxHealth = 100;

        /// <summary>
        /// The starting amount of health
        /// </summary>
        public int startHealth = 100;

        /// <summary>
        /// Whether the object can regenerate health passively
        /// </summary>
        public bool hasRegen = true;

        /// <summary>
        /// The curve used for passive health regeneration<br/>
        /// The curve's value at a certain time is the amount it regenerates per second
        /// </summary>
        public AnimationCurve regenCurve = AnimationCurve.EaseInOut(0, 0, 5, 5);

        /// <summary>
        /// The type of I-Frames used
        /// </summary>
        public IframeTypes iframeType = IframeTypes.Local;

        /// <summary>
        /// The current amount of health<br/>
        /// Use Hit or Regen to edit externally
        /// </summary>
        [field: SerializeField]
        public int healthAmount { get; protected set; } = 100;

        /// <summary>
        /// Whether to destroy the GameObject on death
        /// </summary>
        public bool destroyOnDeath;

        /// <summary>
        /// The delay for destroying the GameObject on death
        /// </summary>
        public float destroyDelay;

        /// <summary>
        /// Whether to destroy the parent GameObject
        /// </summary>
        public bool destroyParent = false;

        /// <summary>
        /// The event invoked on hit, not invoked for fatal hits
        /// </summary>
        public UnityEvent<HitInfo> onHit;

        /// <summary>
        /// The event invoked on death
        /// </summary>
        public UnityEvent<HitInfo> onDeath;

        /// <summary>
        /// Is the object dead<br/>
        /// Use Kill to set it externally
        /// </summary>
        [field: SerializeField]
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

        protected virtual void Awake()
        {
            if (useDataObject && data) {
                mask = data.mask;
                maxHealth = data.maxHealth;
                startHealth = data.startHealth;
                regenCurve = data.regenCurve;
                iframeType = data.iFramesType;
            }

            healthAmount = startHealth;
        }

        /// <summary>
        /// Used to regenerate the object's health by amount
        /// </summary>
        /// <param name="amount">Amount to regenerate the health by</param>
        public virtual void Regen(int amount)
        {
            healthAmount += amount;
            healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);
        }

        /// <summary>
        /// Used to deal damage to the object
        /// </summary>
        /// <param name="info">The HitInfo used for the hit</param>
        public virtual void Hit(HitInfo info)
        {
            if (isDead) return;

            Debug.Log($"{gameObject.name} got hit");

            healthAmount -= info.Damage;
            if (healthAmount <= 0) {
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
            healthAmount = 0;
            onDeath?.Invoke(info);

            if (destroyOnDeath) {
                var objToDestroy = destroyParent ? transform.parent : transform;
                objToDestroy ??= transform;
                Destroy(objToDestroy.gameObject, destroyDelay);
            }
        }

        private void Update()
        {
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
        /// Called by a Hurtbox when it detects a Hitbox
        /// </summary>
        /// <param name="instanceId">The instance ID of the Hitbox</param>
        /// <param name="info">The HitInfo of the Hitbox</param>
        /// <returns>Whether the hit check passed or not</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when iframeType is out of range</exception>
        public virtual bool TryGetHit(int instanceId, HitInfo info)
        {
            if ((mask & info.Mask) == 0) {
                Debug.Log($"Mask {mask} doesn't match {info.Mask}");
                return false;
            }

            switch (iframeType) {
                case IframeTypes.Local: {
                    float iframes = LocalIframes.GetValueOrDefault(instanceId, 0f);
                    if (iframes <= 0) {
                        LocalIframes[instanceId] = info.Iframes;
                        Hit(info);
                        return true;
                    }

                    break;
                }
                case IframeTypes.Global: {
                    if (GlobalIframes <= 0) {
                        GlobalIframes = info.Iframes;
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

        #region Debug Info

#if UNITY_EDITOR
        private void OnGUI()
        {
            if (!CookieDebug.CookieDebug.IsDebugMode) return;
            
            string labelText = $"{healthAmount}/{maxHealth}";
            var worldPos = transform.position;
            worldPos.y *= -1; // negative y for up is used in IMGui
            worldPos -= Vector3.up;
            var labelPos = Camera.main.WorldToScreenPoint(worldPos);
            var rectPos = Rect.MinMaxRect(labelPos.x - 25f, labelPos.y - 25f, labelPos.x + 25f, labelPos.y + 25f);
            GUI.Label(rectPos, labelText);
        }
#endif

        #endregion

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

            public HitInfo(int damage, float iframes, Vector3? direction, int mask = int.MaxValue)
            {
                Damage = damage;
                Iframes = iframes;
                Mask = mask;
                Direction = direction ?? Vector3.right;
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
