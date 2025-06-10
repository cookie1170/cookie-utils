using System.Collections.Generic;
using CookieUtils;
using CookieUtils.Audio;
using CookieUtils.Health;
using CookieUtils.ObjectPooling;
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
        _hitbox = GetComponent<Hitbox>();
        _destroyTimer = this.CreateTimer(5f, onComplete: () =>
        {
            if (!gameObject) return;
            this.Release();
        });
        // _hitbox.onDestroy.AddListener(() =>
        // {
            // if (!gameObject) return;
            // if (_destroyTimer.gameObject.activeInHierarchy)
                // _destroyTimer.Release();
            // this.Release();
        // });
    }
    
    private void OnEnable()
    {
        _rb.linearVelocityX = speed;
        this.PlaySfx(shootSounds.PickRandom(), transform);
        _destroyTimer.Restart();
    }
    }
}
