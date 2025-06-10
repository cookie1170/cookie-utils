using System;
using CookieUtils.ObjectPooling;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace CookieUtils.Health
{
	[RequireComponent(typeof(Collider2D)), DisallowMultipleComponent]
	public class Hitbox : MonoBehaviour
	{
		public enum Types
		{
			Player,
			Enemy,
		}
		
		[SerializeField, Foldout("Properties")]
		public Types type = Types.Enemy;
		
		[SerializeField, Foldout("Properties")]
		public int damage = 20;

		[SerializeField, Foldout("Properties"), Range(0f, 5f)] 
		public float iFrames = 0.2f;

		[SerializeField, Foldout("Properties")]
		public bool hasPierce = true;
		
		[SerializeField, Foldout("Properties"), ShowIf("hasPierce"), Range(1, 20)]
		public int pierceAmount = 1;

		[SerializeField, Foldout("Properties"), ShowIf("hasPierce")]
		public bool destroyWhenNoPierce = true;

		[SerializeField, Foldout("References")]
		private Collider2D trigger;

		[Foldout("Events")] public UnityEvent onDestroy;
		
		private int _pierceLeft;

		private void Awake() => OnValidate();

		private void OnValidate()
		{
			if (!trigger) trigger = GetComponentInParent<Collider2D>();
			
			trigger.isTrigger = true;
			trigger.gameObject.layer = LayerMask.NameToLayer($"{type} hitboxes");
		}

		private void OnEnable()
		{
			_pierceLeft = pierceAmount;
		}

		public void OnAttack()
		{
			if (!hasPierce) return;
			_pierceLeft--;
			if (_pierceLeft <= 0)
			{
				onDestroy?.Invoke();
				if (destroyWhenNoPierce)
				{
					Transform objToDestroy = transform.parent ?? transform;
					if (!objToDestroy.Release()) Destroy(objToDestroy.gameObject);
				}
			}
		}
	}
}
