using System;
using System.Collections.Generic;
using CookieUtils.Audio;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace CookieUtils.Health
{
    [DisallowMultipleComponent]
    public class Health : MonoBehaviour
    {
        [Foldout("Properties")] public int maxHealth = 100;
        [SerializeField, Foldout("Properties")] private bool destroyOnDeath = true;
        
        [Space(10f)]
        [SerializeField, Foldout("References")] private ParticleSystem hitParticles;
        [SerializeField, Foldout("References")] private ParticleSystem deathParticles;
        [SerializeField, Foldout("References")] private Healthbar.Healthbar healthbar;
        
        [Space(10f)]
        [SerializeField, Foldout("References")] private List<AudioClip> hurtSounds;
        [SerializeField, Foldout("References")] private List<AudioClip> deathSounds;
        
        [Space(10f)]
        [Foldout("Events")] public UnityEvent<int> onHit;
        [Foldout("Events")] public UnityEvent onDeath;

        public int Amount
        {
            get => _amount;
            set => _amount = Math.Clamp(value, 0, maxHealth);
        }
        
        private int _amount;

        private void Awake() => Amount = maxHealth;

        public void OnHit(int damageAmount, float hitAngle = 0f)
        {
            Amount -= Math.Max(1, damageAmount); // ensure damageAmount can't go below 1
            onHit?.Invoke(Amount);
            if (Amount <= 0)
            {
                onDeath?.Invoke();
                if (deathSounds != null && deathSounds.Count > 0)
                    this.PlaySfx(deathSounds.PickRandom(), transform);
                if (destroyOnDeath) Destroy(gameObject);
            }
            else if (hurtSounds != null && hurtSounds?.Count > 0)
                this.PlaySfx(hurtSounds.PickRandom(), transform);

            if (healthbar != null)
            {
                healthbar.maxValue = maxHealth;
                healthbar.Value = Amount;
            }

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
    }
}
