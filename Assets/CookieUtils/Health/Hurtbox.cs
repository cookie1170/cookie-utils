using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;

namespace CookieUtils.Health
{
    [RequireComponent(typeof(Collider2D)), DisallowMultipleComponent]
    public class Hurtbox : MonoBehaviour
    {
        [SerializeField, Foldout("Properties")]
        private Hitbox.Types type = Hitbox.Types.Enemy;

        [SerializeField, Foldout("Properties"), Range(0, 5f)]
        private float iFrameMult = 1f;
        
        [SerializeField, Foldout("References")]
        private Collider2D trigger;

        [SerializeField, Foldout("References")]
        private Health health;
            
        private readonly List<Hitbox> _collidingHitboxes = new();
        private readonly Dictionary<Hitbox, float> _iFrames = new();

        private void Awake() => OnValidate();
        
        private void OnValidate()
        {
            if (!trigger) trigger = GetComponent<Collider2D>();
            if (!health) health = GetComponentInParent<Health>();
            
            trigger.isTrigger = true;
            int layerMask = LayerMask.GetMask(type == Hitbox.Types.Enemy ? "Player hitboxes" : "Enemy hitboxes"); 
            trigger.excludeLayers = int.MaxValue - layerMask;
            trigger.includeLayers = layerMask;
        }

        private void OnDisable()
        {
            _collidingHitboxes.Clear();
            _iFrames.Clear();
        }

        private void Update()
        {
            Hitbox[] keys = Enumerable.ToArray(_iFrames.Keys);
            
            for (int i = 0; i < keys.Length; i++)
            {
                Hitbox key = keys[i];
                float value = _iFrames[key];
                value -= Time.deltaTime;
                _iFrames[key] = value;
                if (value <= 0f)
                    _iFrames.Remove(key);
            }
            
            foreach (Hitbox hitbox in _collidingHitboxes)
                GetHit(hitbox);
        }

        private void GetHit(Hitbox hitbox)
        {
            if (_iFrames.ContainsKey(hitbox)) return;
            
            _iFrames.Add(hitbox, hitbox.iFrames * iFrameMult);
            health.GetHit(hitbox);
            hitbox.OnAttack();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Hitbox hitbox))
                _collidingHitboxes.Add(hitbox);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Hitbox hitbox))
                _collidingHitboxes.Remove(hitbox);
        }
    }
}