using System;
using UnityEngine;

namespace CookieUtils.StateMachines
{
    [Serializable]
    public abstract class State<T>
    {
        public GameObject gameObject;

        [NonSerialized] protected internal T Host;
        protected internal StateMachine<T> StateMachine;

        internal void Init(StateMachine<T> stateMachine)
        {
            StateMachine = stateMachine;
            gameObject?.SetActive(false);
        }

        public virtual void Start()
        {
        }

        public virtual void Leave()
        {
        }

        public virtual void Enter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }
    }
}