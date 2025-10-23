using UnityEngine;
using UnityEngine.InputSystem;

namespace Samples.Health
{
    public class SamplePlayer : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D bulletPrefab;
        [SerializeField] private InputActionReference fireAction;

        private void Awake()
        {
            fireAction.action.performed += Fire;
            InputSystem.actions.FindActionMap("Player").Enable();
        }

        private void Fire(InputAction.CallbackContext ctx)
        {
            Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.linearVelocity = Vector2.right * 7.5f;
        }
    }
}