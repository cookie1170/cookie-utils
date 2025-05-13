using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CookieUtils.Health
{
    [RequireComponent(typeof(Collider2D)), DisallowMultipleComponent]
    public class Hitbox : MonoBehaviour
    {
        [Foldout("Properties")] public bool isOneShot = false;
        [Foldout("Properties"), HideIf("isOneShot")] public int damage = 20;
        [Foldout("Properties")] public float iFrames = 0.2f;
        
        [Foldout("Properties")] public Vector2 direction = Vector2.right;
        [field: SerializeField, Foldout("Properties"), Space(10f)] public Types Type { get; private set; } = Types.Enemy;
        [SerializeField, Foldout("Properties")] private Collider2D trigger;
        
        [Space(10f)]
        [SerializeField, Foldout("Properties")] private bool hasPierce = true;
        [SerializeField, Foldout("Properties"), ShowIf("hasPierce")] private int pierceAmount = 1;

        [Space(10f)]
        [Foldout("Events")] public UnityEvent onAttack;
        [Foldout("Events")] public UnityEvent onDestroy;
        
        private int _pierceLeft;

        public enum Types
        {
            Player,
            Enemy
        }

        private void Awake()
        {
            if (trigger == null) trigger = GetComponent<Collider2D>();
            trigger.isTrigger = true;
            _pierceLeft = pierceAmount;
            onAttack.AddListener(OnAttack);
        }

        private void OnAttack()
        {
            if (!hasPierce) return;
            _pierceLeft--;
            if (_pierceLeft <= 0) onDestroy.Invoke();
        }
    }
}