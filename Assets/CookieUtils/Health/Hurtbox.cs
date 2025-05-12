using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CookieUtils.Health
{
    [RequireComponent(typeof(Health)), RequireComponent(typeof(Collider2D)), DisallowMultipleComponent]
    public class Hurtbox : MonoBehaviour
    {
        [SerializeField] private float iFrameMultiplier = 1f;
        [SerializeField] private Hitbox.Types type = Hitbox.Types.Enemy;
        [SerializeField] private Collider2D trigger;
        
        public UnityEvent<int, float> onHit;
        
        private Health _health;
        private readonly Dictionary<Hitbox, Timer.Timer> _iFrames = new();

        private void Awake()
        {
            if (trigger == null) trigger = GetComponent<Collider2D>();
            _health = GetComponent<Health>();
            trigger.isTrigger = true;
            onHit.AddListener(_health.OnHit);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Hitbox hitbox)) return;
            if (_iFrames.ContainsKey(hitbox)) return;
            if (hitbox.type == type) return;
            GetHit(hitbox);
        }

        private void GetHit(Hitbox hitbox)
        {
            _iFrames.Add(hitbox, this.CreateTimer(hitbox.iFrames * iFrameMultiplier, onComplete: () =>
                _iFrames.Remove(hitbox)));
            hitbox.onAttack.Invoke();
            float hitAngle = Vector2.SignedAngle(Vector2.right, hitbox.direction);
            onHit.Invoke(hitbox.damage, hitAngle);
        }
    }
}
