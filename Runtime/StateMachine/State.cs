using System;
using UnityEngine;

namespace CookieUtils.StateMachine
{
    [Serializable]
    public abstract class State<T> where T : MonoBehaviour
    {
        [field: SerializeField] public GameObject GameObject { get; private set; }

        [NonSerialized] protected internal T Host;
        protected internal StateMachine<T> StateMachine;

        public void Init(StateMachine<T> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Leave() { }
        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}