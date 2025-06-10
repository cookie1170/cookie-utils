using UnityEngine;
using CookieUtils;
using CookieUtils.ObjectPooling;
using CookieUtils.Timer;

namespace Sample
{
    public class SampleEnemy : MonoBehaviour
    {
        [SerializeField] private float cooldown = 2.5f;
        [SerializeField] private GameObject bullet;

        private Timer _shootTimer;
        
        private void Start()
        {
            _shootTimer = this.CreateTimer(cooldown, repeat: true,
                onComplete: () => bullet.Get(transform.position, Quaternion.Euler(0, 0, 90)));
        }
    }
}
