using System;
using CookieUtils.ObjectPooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CookieUtils.Samples
{
    public class ObjectPoolingSampleObject : MonoBehaviour, IPoolCallbackReceiver
    {
        [SerializeField] private SpriteRenderer sprite;
        private float _vel = 0;
        public Action OnReleaseAction;

        private void FixedUpdate() {
            _vel -= Time.fixedDeltaTime;
            transform.position = transform.position.Add(y: _vel);

            if (transform.position.y < -5f) this.Release();
        }

        public void OnGet() {
            sprite.color = Random.ColorHSV(0, 1, 0, 0.75f, 0.1f, 0.8f);
            _vel = 0;
        }

        public void OnRelease() {
            OnReleaseAction();
        }
    }
}