using System;
using UnityEngine;
using UnityEngine.Events;

namespace CookieUtils.Health
{
    [DisallowMultipleComponent]
    public class Health : MonoBehaviour
    {
        public int maxHealth = 100;
        [SerializeField] private bool destroyOnDeath = true;
        [SerializeField] private ParticleSystem hitParticles;
        [SerializeField] private ParticleSystem deathParticles;

        public UnityEvent<int> onHit;
        public UnityEvent onDeath;

        public int Amount
        {
            get => _amount;
            set => _amount = Math.Clamp(value, 0, maxHealth);
        }

        [SerializeField] private Healthbar.Healthbar healthbar;

        private int _amount;

        private void Awake() => Amount = maxHealth;

        public void OnHit(int damageAmount, float hitAngle = 0f)
        {
            Amount -= Math.Max(1, damageAmount); // ensure damageAmount can't go below 1
            onHit?.Invoke(Amount);
            if (Amount <= 0)
            {
                onDeath?.Invoke();
                if (destroyOnDeath) Destroy(gameObject);
            }

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
