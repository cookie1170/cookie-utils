using System.Collections.Generic;
using CookieUtils;
using CookieUtils.Audio;
using CookieUtils.Health;
using CookieUtils.Timer;
using UnityEngine;

namespace Sample
{
    public class SampleBullet : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private List<AudioClip> shootSounds;
        
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
            this.PlaySfx(shootSounds.PickRandom(), transform);
        }

        private void OnDestroy()
        {
            if (_destroyTimer == null) return;
            _destroyTimer.OnComplete = null;
            _destroyTimer.Release();
        }
    }
}
