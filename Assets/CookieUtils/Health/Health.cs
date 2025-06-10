using System;
using CookieUtils.ObjectPooling;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace CookieUtils.Health
{
	public class Health : MonoBehaviour
	{
		public int Amount
		{
			get => _amount;
			set
			{
				_amount = Math.Clamp(value, 0, maxHealth);
				if (healthBar)
				{
					healthBar.maxValue = maxHealth;
					healthBar.Value = _amount;
				}

				if (_amount <= 0) OnDeath();
			}
		}
		
		[SerializeField, Foldout("Properties"), Min(1)]
		public int maxHealth = 100;

		[SerializeField, Foldout("Properties")]
		public bool destroyOnDeath;

		[SerializeField, Foldout("References")]
		public Healthbar.Healthbar healthBar;

		[Foldout("Events")]
		public UnityEvent<Hitbox> onHit;
		[Foldout("Events")]
		public UnityEvent onDeath;

		[ShowNonSerializedField] private int _amount;

		private void Awake() => OnValidate();
		
		private void OnValidate()
		{
			if (!healthBar) healthBar = GetComponentInChildren<Healthbar.Healthbar>();
			if (!healthBar) healthBar = GetComponentInParent<Healthbar.Healthbar>();
			Amount = maxHealth;
		}
		
		private void OnEnable()
		{
			Amount = maxHealth;
		}

		public void GetHit(Hitbox hitbox)
		{
			onHit?.Invoke(hitbox);
			Amount -= hitbox.damage;
		}

		private void OnDeath()
		{
			onDeath?.Invoke();
			if (destroyOnDeath)
			{
				Transform objToDestroy = transform.parent ?? transform;
				if (!objToDestroy.Release()) Destroy(objToDestroy.gameObject);
			}
		}
	}
}
