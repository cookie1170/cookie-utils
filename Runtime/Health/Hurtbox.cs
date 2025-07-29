using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;

namespace CookieUtils.Runtime.Health
{
    [RequireComponent(typeof(Collider2D)), DisallowMultipleComponent]
    public class Hurtbox : MonoBehaviour
    {
        [SerializeField, Foldout("Properties")]
        private Hitbox.Types type = Hitbox.Types.Enemy;

        [SerializeField, Foldout("Properties"), Range(0, 5f)]
        private float iFrameMult = 1f;

        [SerializeField, Foldout("References")]
        private bool overrideTrigger = false;
        
        [SerializeField, Foldout("References"), ShowIf("overrideTrigger")]
        private Collider2D trigger;

        [SerializeField, Foldout("References")]
        private bool overrideHealth;

        [SerializeField, Foldout("References"), ShowIf("overrideHealth")]
        private Health health;
            
        private readonly List<Hitbox> _collidingHitboxes = new();
        private readonly Dictionary<Hitbox, float> _iFrames = new();

        private void Awake() => OnValidate();
        
        private void OnValidate()
        {
            if (!overrideTrigger) trigger = GetComponent<Collider2D>();
            if (!overrideHealth) health = GetComponentInParent<Health>();
            
            trigger.isTrigger = true;
            LayerMask layerMask = LayerMask.GetMask(type == Hitbox.Types.Enemy ? "Player hitboxes" : "Enemy hitboxes"); 
            trigger.excludeLayers = int.MaxValue - layerMask;
            trigger.includeLayers = layerMask;
            trigger.contactCaptureLayers = layerMask;
            trigger.callbackLayers = layerMask;
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

            for (int i = 0; i < _collidingHitboxes.Count; i++)
            {
                Hitbox hitbox = _collidingHitboxes[i];
                GetHit(hitbox);
            }
        }

        private void GetHit(Hitbox hitbox)
        {
            if (_iFrames.ContainsKey(hitbox)) return;
            if (!hitbox) return;
            
            _iFrames.Add(hitbox, hitbox.iFrames * iFrameMult);
            health.GetHit(hitbox);
            hitbox.OnAttack();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Hitbox hitbox))
                if (hitbox.type != type) _collidingHitboxes.Add(hitbox);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Hitbox hitbox))
                _collidingHitboxes.Remove(hitbox);
        }
    }
}