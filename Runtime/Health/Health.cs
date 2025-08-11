using System;
using System.Collections;
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

		[SerializeField, Range(0, 5), Foldout("Properties"), ShowIf("destroyOnDeath")]
		public float destroyDelay;

		[SerializeField, Foldout("References")]
		public bool overrideHurtEffects;

		[SerializeField, Foldout("References"), ShowIf("overrideHurtEffects")]
		public HurtEffect[] hurtEffects;

		[SerializeField, Foldout("References")]
		private bool useHealthBar = true;
		
		[SerializeField, Foldout("References"), ShowIf("useHealthBar")]
		public Healthbar.Healthbar healthBar;

		[Space(10f)]
		[SerializeField, Foldout("References")]
		public bool hurtDirectional = true;
		
		[SerializeField, Foldout("References"), ShowIf("hurtDirectional"), Tooltip("Direction relative to which the hurt particles are rotated")]
		public Vector2 hurtDirection = Vector2.right;
		
		[SerializeField, Foldout("References")]
		public ParticleSystem hurtParticles;
		
		[SerializeField, Foldout("References")]
		public AudioClip[] hurtSounds;
		
		[Space(10f)]
		[SerializeField, Foldout("References")]
		public bool deathDirectional = true;
		
		[SerializeField, Foldout("References"), ShowIf("deathDirectional"), Tooltip("Direction relative to which the death particles are rotated")]
		public Vector2 deathDirection = Vector2.right;

		[SerializeField, Foldout("References")]
		public ParticleSystem deathParticles;
		
		[SerializeField, Foldout("References")]
		public AudioClip[] deathSounds;

		[Foldout("Events")]
		public UnityEvent<Hitbox> onHit;
		[Foldout("Events")]
		public UnityEvent<Hitbox> onDeath;

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
				StartCoroutine(OnDeath(hitbox));
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

		private IEnumerator OnDeath(Hitbox hitbox)
		{
			if (_isDead) yield break;
			_isDead = true;

			onDeath?.Invoke(hitbox);
			Quaternion rotation = deathDirectional
				? Quaternion.Euler(0, 0, Vector2.SignedAngle(deathDirection, hitbox.Direction))
				: Quaternion.identity;
			
			if (deathParticles)
				deathParticles.GetObj(transform.position, rotation);
			
			if (deathSounds.Length > 0)
				this.PlaySfx(deathSounds.PickRandom());
			
			if (destroyOnDeath)
			{
				if (destroyDelay > 0) yield return new WaitForSeconds(destroyDelay);
				Transform objToDestroy = transform.parent ?? transform;
				if (!objToDestroy.Release()) Destroy(objToDestroy.gameObject);
			}
		}
	}
}
