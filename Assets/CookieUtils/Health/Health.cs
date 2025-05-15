using System;
using System.Collections.Generic;
using CookieUtils.Audio;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CookieUtils.Health
{
    [DisallowMultipleComponent]
    public class Health : MonoBehaviour
    {
        [Foldout("Properties")] public int maxHealth = 100;
        [SerializeField, Foldout("Properties")] private bool destroyOnDeath = true;
        [Foldout("Properties")] public bool hasHealthRegen;

        [Foldout("Properties"), ShowIf(("hasHealthRegen")),
         Tooltip("Amount of health regenerated per second at full speed")]
        public float healthRegen = 2.5f;

        [SerializeField, Foldout("Properties"), ShowIf("hasHealthRegen"),
         Tooltip("The curve used for ramping up health regen after getting hit")]
        private ParticleSystem.MinMaxCurve rampUpAmount;

        [SerializeField, Foldout("Properties"), ShowIf("hasHealthRegen"),
         Tooltip("The time it takes for the healing to ramp up to full after getting hit")]
        private float rampUpTime = 3f;

        [SerializeField, Foldout("Properties"), ShowIf("hasHealthRegen"),
         Tooltip("The delay after getting hit before healing starts")]
        private float healDelay = 2f;
        
        
        [Space(10f)]
        [SerializeField, Foldout("References")] private ParticleSystem hitParticles;
        [SerializeField, Foldout("References")] private ParticleSystem deathParticles;
        [SerializeField, Foldout("References")] private Healthbar.Healthbar healthbar;
        
        [Space(10f)]
        [SerializeField, Foldout("References")] private List<AudioClip> hurtSounds;
        [SerializeField, Foldout("References")] private List<AudioClip> deathSounds;
        
        [Space(10f)]
        [Foldout("Events")] public UnityEvent<float> onHit;
        [Foldout("Events")] public UnityEvent onDeath;

        public float Amount
        {
            get => _amount;
            set
            {
                _amount = Math.Clamp(value, 0, maxHealth);
                
                if (healthbar != null)
                {
                    healthbar.maxValue = maxHealth;
                    healthbar.Value = Amount;
                }
            }
        }

        private float _amount;
        private float _timeSinceHit;

        private void Awake() => Amount = maxHealth;

        public void OnHit(int damageAmount, float hitAngle = 0f)
        {
            Amount -= Math.Max(1, damageAmount); // ensure damageAmount can't go below 1
            _timeSinceHit = 0;
            onHit?.Invoke(Amount);
            if (Amount <= 0)
            {
                onDeath?.Invoke();
                if (deathSounds is { Count: > 0 })
                    this.PlaySfx(deathSounds.PickRandom(), transform);
                if (destroyOnDeath) Destroy(gameObject);
            }
            else if (hurtSounds is { Count: > 0 })
                this.PlaySfx(hurtSounds.PickRandom(), transform);

            if (Amount > 0)
            {
                if (hitParticles != null)
                    Instantiate(hitParticles, transform.position, Quaternion.Euler(0, 0, hitAngle));
            }
            else
            {
                if (deathParticles != null)
                    Instantiate(deathParticles, transform.position, Quaternion.Euler(0, 0, hitAngle));
            }
        }

        public void Update()
        {
            if (!hasHealthRegen) return;
            
            _timeSinceHit += Time.deltaTime;
            
            if (_timeSinceHit - healDelay < rampUpTime)
            {
                float progress = Mathf.Clamp01((_timeSinceHit - healDelay) / rampUpTime);
                Amount += rampUpAmount.Evaluate(progress) * healthRegen * Time.deltaTime;
            }
            else Amount += healthRegen * Time.deltaTime;
        }
    }
}