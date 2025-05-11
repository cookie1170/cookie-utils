using System;
using CookieUtils.Health;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sample
{
    public class SamplePlayer : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private InputActionReference action;
        
        private Health _health;
        
        private void Awake()
        {
            _health = GetComponent<Health>();
            _health.onDeath.AddListener(_ => Destroy(gameObject));
        }

        private void Update()
        {
            if (action.action.WasPerformedThisFrame())
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, -90));
            }
        }
    }
}
