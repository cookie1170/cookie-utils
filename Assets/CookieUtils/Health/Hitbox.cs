using UnityEngine;
using UnityEngine.Events;   

namespace CookieUtils.Health
{
    [RequireComponent(typeof(Collider2D)), DisallowMultipleComponent]
    public class Hitbox : MonoBehaviour
    {
        public int damage = 20;
        public float iFrames = 0.2f;
        public Types type = Types.Enemy;
        public Vector2 direction = Vector2.right;
        public UnityEvent onAttack;
        public UnityEvent onDestroy;

        [SerializeField] private int pierceAmount = 1;
        [SerializeField] private Collider2D trigger;

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
            _pierceLeft--;
            if (_pierceLeft <= 0) onDestroy.Invoke();
        }
    }
}