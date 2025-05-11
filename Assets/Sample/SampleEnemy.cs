using System;
using UnityEngine;
using CookieUtils;
using CookieUtils.Health;
using CookieUtils.Timer;

namespace Sample
{
    public class SampleEnemy : MonoBehaviour
    {
        [SerializeField] private float cooldown = 2.5f;
        [SerializeField] private GameObject bullet;

        private Timer _shootTimer;
        
        private void Awake()
        {
            _shootTimer = this.CreateTimer(cooldown, repeat: true,
                onComplete: () => Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 90)));
            GetComponent<Health>().onDeath.AddListener(v => Destroy(gameObject));
        }

        private void OnDestroy() => _shootTimer.Release();
    }
}
