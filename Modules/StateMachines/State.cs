using System;
using CookieUtils.Debugging;
using UnityEngine;

namespace CookieUtils.StateMachines
{
    [Serializable]
    public abstract class State<THost>
    {
        /// <summary>
        /// The game object linked to this state. Active when the state is current
        /// </summary>
        [Tooltip("The game object linked to this state. Active when the state is current")]
        public GameObject gameObject;

        /// <summary>
        /// The host of this state's state machine
        /// </summary>
        protected internal THost Host;

        /// <summary>
        /// The state machine that this state belongs to
        /// </summary>
        protected internal StateMachine<THost> StateMachine;

        internal void Init(StateMachine<THost> stateMachine)
        {
            StateMachine = stateMachine;
            if (gameObject)
                gameObject.SetActive(false);
        }

        /// <summary>
        /// Called after all the states have been set up, but before any state has been entered
        /// </summary>
        internal void Start() => OnStart();

        /// <summary>
        /// Called when the state machine changes state from this one
        /// </summary>
        internal void Leave(bool keepObjectActive)
        {
            if (!keepObjectActive && gameObject)
                gameObject.SetActive(false);

            OnLeave();
        }

        /// <summary>
        /// Call to enter this state
        /// </summary>
        /// <param name="keepObjectActive">Whether to keep the previous state's <see cref="gameObject"/> active </param>
        public void Enter(bool keepObjectActive)
        {
            if (gameObject)
                gameObject.SetActive(true);

            StateMachine.ChangeState(this, keepObjectActive);

            OnEnter();
        }

        /// <inheritdoc cref="Enter(bool)"/>
        public void Enter() => Enter(false);

        /// <summary>
        /// Called by the state machine during update
        /// </summary>
        internal void Update() => OnUpdate();

        /// <summary>
        /// Called by the state machine during fixed update
        /// </summary>
        internal void FixedUpdate() => OnFixedUpdate();

        /// <summary>
        /// Called by the state machine to set up debug ui drawing
        /// </summary>
        /// <param name="builder">The debug ui builder for this state</param>
        internal void SetUpDebugUIInternal(IDebugUI_Builder builder) => SetUpDebugUI(builder);

        /// <summary>
        /// Called when this state is entered
        /// </summary>
        protected virtual void OnEnter() { }

        /// <inheritdoc cref="Start"/>
        protected virtual void OnStart() { }

        /// <inheritdoc cref="Leave"/>
        protected virtual void OnLeave() { }

        /// <inheritdoc cref="Update"/>
        protected virtual void OnUpdate() { }

        /// <inheritdoc cref="FixedUpdate"/>
        protected virtual void OnFixedUpdate() { }

        /// <inheritdoc cref="SetUpDebugUIInternal"/>
        protected virtual void SetUpDebugUI(IDebugUI_Builder builder) { }

        /// <summary>
        /// Gets the state <c>TState</c>
        /// </summary>
        /// <typeparam name="TState">The type of state to get</typeparam>
        /// <returns>The state of type <c>TState</c> on the host state machine</returns>
        protected TState Get<TState>()
            where TState : State<THost> => StateMachine.Get<TState>();
    }
}
