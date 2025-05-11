using System;
using UnityEngine;
using UnityEngine.Events;

namespace CookieUtils.Health
{
    [DisallowMultipleComponent]
    public class Health : MonoBehaviour
    {
        public int maxHealth = 100;

        public UnityEvent<int> onHit;
        public UnityEvent<float> onDeath;
        
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
            Amount -= damageAmount;
            onHit?.Invoke(Amount);
            if (Amount <= 0) onDeath?.Invoke(hitAngle);
            if (healthbar != null)
            {
                healthbar.maxValue = maxHealth;
                healthbar.Value = Amount;
            }
        }
    }
}
