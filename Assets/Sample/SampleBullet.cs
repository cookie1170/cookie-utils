using System;
using CookieUtils;
using CookieUtils.Health;
using CookieUtils.Timer;
using UnityEngine;

namespace Sample
{
    public class SampleBullet : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        
        private Rigidbody2D _rb;
        private Hitbox _hitbox;

        private Timer _destroyTimer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.linearVelocityX = speed;
            _hitbox = GetComponent<Hitbox>();
            _hitbox.onDestroy.AddListener(() =>
            {
                if (gameObject != null) Destroy(gameObject);
            });
            _destroyTimer = this.CreateTimer(5f, onComplete: () =>
            {
                if (gameObject != null) Destroy(gameObject);
            });
        }

        private void OnDestroy()
        {
            _destroyTimer.OnComplete = null;
        }
    }
}
