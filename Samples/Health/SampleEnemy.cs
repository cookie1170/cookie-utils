using UnityEngine;

namespace Samples.Health
{
	public class SampleEnemy : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D bulletPrefab;

		private float _cooldown = 2;

		private void Update()
		{
			_cooldown -= Time.deltaTime;

			if (_cooldown < 0)
			{
				_cooldown = 2;
				var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.linearVelocity = Vector2.left * 5f;
            }
		}
	}
}
