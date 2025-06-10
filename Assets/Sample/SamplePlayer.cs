using CookieUtils.ObjectPooling;
using CookieUtils.Timer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sample
{
    public class SamplePlayer : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private InputActionReference action;

        private Timer _cooldownTimer;

        private void Start()
        {
            _cooldownTimer = this.CreateTimer(0.5f, destroyOnFinish: false, ignoreNullAction: true);
        }

        private void Update()
        {
            if (action.action.WasPerformedThisFrame() && !_cooldownTimer.IsRunning)
            {
                    bulletPrefab.Get(transform.position, Quaternion.Euler(0, 0, -90));
                _cooldownTimer.Restart();
            }
        }
    }
}
