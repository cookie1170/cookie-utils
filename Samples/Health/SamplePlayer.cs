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
			var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, -90));
            bullet.linearVelocity = Vector2.right * 7.5f;
        }
	}
}
