using System;
using CookieUtils.Runtime.Audio;
using CookieUtils.Runtime.ObjectPooling;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace CookieUtils.Runtime.Health
{
	public class Health : MonoBehaviour
	{
		[ShowNonSerializedField] private int _amount;
		
		public int Amount
		{
			get => _amount;
			set
			{
				_amount = Math.Clamp(value, 0, maxHealth);
				if (healthBar && useHealthBar)
				{
					healthBar.maxValue = maxHealth;
					healthBar.Value = _amount;
				}
			}
		}
		
		[SerializeField, Foldout("Properties"), Min(1)]
		public int maxHealth = 100;

		[SerializeField, Foldout("Properties")]
		public bool destroyOnDeath;

		[SerializeField, Foldout("References")]
		private bool overrideHurtEffects;

		[SerializeField, Foldout("References"), ShowIf("overrideHurtEffects")]
		private HurtEffect[] hurtEffects;

		[SerializeField, Foldout("References")]
		private bool useHealthBar = true;
		
		[SerializeField, Foldout("References"), ShowIf("useHealthBar")]
		public Healthbar.Healthbar healthBar;

		[Space(10f)]
		[SerializeField, Foldout("References")]
		private bool hurtDirectional = true;
		
		[SerializeField, Foldout("References"), ShowIf("hurtDirectional"), Tooltip("Direction relative to which the hurt particles are rotated")]
		private Vector2 hurtDirection = Vector2.right;
		
		[SerializeField, Foldout("References")]
		private ParticleSystem hurtParticles;
		
		[SerializeField, Foldout("References")]
		private AudioClip[] hurtSounds;
		
		[Space(10f)]
		[SerializeField, Foldout("References")]
		private bool deathDirectional = true;
		
		[SerializeField, Foldout("References"), ShowIf("deathDirectional"), Tooltip("Direction relative to which the death particles are rotated")]
		private Vector2 deathDirection = Vector2.right;

		[SerializeField, Foldout("References")]
		private ParticleSystem deathParticles;
		
		[SerializeField, Foldout("References")]
		private AudioClip[] deathSounds;

		[Foldout("Events")]
		public UnityEvent<Hitbox> onHit;
		[Foldout("Events")]
		public UnityEvent onDeath;

		private bool _isDead;
		
		private void Awake() => OnValidate();
		
		private void OnValidate()
		{
			if (!healthBar && useHealthBar) healthBar = GetComponentInChildren<Healthbar.Healthbar>();
			if (!healthBar && useHealthBar) healthBar = GetComponentInParent<Healthbar.Healthbar>();
			if (!overrideHurtEffects)
			{
				hurtEffects = transform.parent
					? transform.parent.GetComponentsInChildren<HurtEffect>()
					: GetComponentsInChildren<HurtEffect>();
			}
			Amount = maxHealth;
		}
		
		private void OnEnable()
		{
			Amount = maxHealth;
		}

		public void GetHit(Hitbox hitbox)
		{
			foreach (HurtEffect effect in hurtEffects)
				effect.OnHit(hitbox.direction);
			
			onHit?.Invoke(hitbox);
			Amount -= hitbox.damage;
			if (Amount <= 0)
			{
				if (healthBar && useHealthBar)
					healthBar.Value = 0f;
				OnDeath(hitbox.Direction);
				return;
			}

			if (hurtParticles)
			{
				Quaternion rotation = hurtDirectional
					? Quaternion.Euler(0, 0, Vector2.SignedAngle(hurtDirection, hitbox.Direction))
					: Quaternion.identity;
				
				hurtParticles.GetObj(transform.position, rotation);
			}
			if (hurtSounds.Length > 0)
				this.PlaySfx(hurtSounds.PickRandom());
		}

		private void OnDeath(Vector2 direction)
		{
			if (_isDead) return;
			_isDead = true;
			
			onDeath?.Invoke();
			Quaternion rotation = deathDirectional
				? Quaternion.Euler(0, 0, Vector2.SignedAngle(deathDirection, direction))
				: Quaternion.identity;
			
			if (deathParticles)
				deathParticles.GetObj(transform.position, rotation);
			
			if (deathSounds.Length > 0)
				this.PlaySfx(deathSounds.PickRandom());
			
			if (destroyOnDeath)
			{
				Transform objToDestroy = transform.parent ?? transform;
				if (!objToDestroy.Release()) Destroy(objToDestroy.gameObject);
			}
		}
	}
}
