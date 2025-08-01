using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;

namespace CookieUtils.Runtime.Health
{
    [RequireComponent(typeof(Collider2D)), DisallowMultipleComponent]
    public class Hurtbox : MonoBehaviour
    {
        private enum IframeTypes
        {
            Local,
            Global
        }
        
        [SerializeField, Foldout("Properties")]
        private Hitbox.Types type = Hitbox.Types.Enemy;

        [SerializeField, Foldout("Properties")]
        private IframeTypes iFrameType = IframeTypes.Local;
        
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
        private readonly Dictionary<Hitbox, float> _localIframes = new();
        private float _globalIframes;

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
            _localIframes.Clear();
        }

        private void Update()
        {
            if (iFrameType == IframeTypes.Global)
            {
                _globalIframes -= Time.deltaTime;
            }
            else
            {
                Hitbox[] keys = Enumerable.ToArray(_localIframes.Keys);

                for (int i = 0; i < keys.Length; i++)
                {
                    Hitbox key = keys[i];
                    float value = _localIframes[key];
                    value -= Time.deltaTime;
                    _localIframes[key] = value;
                    if (value <= 0f)
                        _localIframes.Remove(key);
                }
            }

            for (int i = 0; i < _collidingHitboxes.Count; i++)
            {
                Hitbox hitbox = _collidingHitboxes[i];
                GetHit(hitbox);
            }

        }

        private void GetHit(Hitbox hitbox)
        {
            if (HasIframes(hitbox)) return;
            if (!hitbox) return;

            switch (iFrameType)
            {
                default:
                    _localIframes.Add(hitbox, hitbox.iFrames * iFrameMult);
                    break;
                case IframeTypes.Global:
                    _globalIframes = hitbox.iFrames * iFrameMult;
                    break;
            }
            
            health.GetHit(hitbox);
            hitbox.OnAttack();
        }

        private bool HasIframes(Hitbox hitbox)
        {
            switch (iFrameType)
            {
                default: return _localIframes.ContainsKey(hitbox);
                case IframeTypes.Global: return _globalIframes > 0.0f;
            }
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