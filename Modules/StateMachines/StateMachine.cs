using System;
using System.Collections.Generic;
using CookieUtils.Base;
using CookieUtils.Debugging;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CookieUtils.StateMachines
{
    [PublicAPI]
    public class StateMachine<T> : IUpdate, IFixedUpdate, IDisposable
    {
        private readonly T _host;

        private readonly Dictionary<Type, State<T>> _states = new();

        public StateMachine(T host, Type defaultState, params State<T>[] states)
        {
            Updater.Register(this);

            if (host is Object obj)
            {
                Updater.AddTo(this, obj);
            }

            _host = host;

            foreach (State<T> state in states)
            {
                _states.Add(state.GetType(), state);
                state.Host = host;
                state.Init(this);
            }

            ChangeState(defaultState);

            foreach (State<T> state in states)
                state.Start();
        }

        public State<T> CurrentState { get; private set; }

        /// <summary>
        ///     Called automatically during PlayerLoop.Update
        /// </summary>
        public void Update()
        {
            CurrentState.Update();
        }

        /// <summary>
        ///     Called automatically during PlayerLoop.FixedUpdate
        /// </summary>
        public void FixedUpdate()
        {
            CurrentState.FixedUpdate();
        }

        private string GetHostName()
        {
            if (_host is MonoBehaviour mb)
                return mb.name;

            return typeof(T).Name;
        }

        public void ChangeState(Type state, bool keepObjectActive = false)
        {
            if (!_states.TryGetValue(state, out State<T> newState))
            {
                Debug.LogWarning(
                    $"State machine under owner {GetHostName()} does not include state of type {state}"
                );

                return;
            }

            if (CurrentState != null)
            {
                CurrentState.Leave();
                if (CurrentState.gameObject)
                    CurrentState.gameObject.SetActive(keepObjectActive);
            }

            if (newState.gameObject)
                newState.gameObject.SetActive(true);

            newState.Enter();
            CurrentState = newState;
        }

        public void ChangeState<TState>(bool keepObjectActive = false)
        {
            ChangeState(typeof(TState), keepObjectActive);
        }

        public void Dispose()
        {
            Updater.Deregister(this);
        }

        /// <summary>
        /// Call this method in <see cref="IDebugDrawer.SetUpDebugUI"/> with an IDebugUI_Builder (can be a group)
        /// </summary>
        /// <param name="builder">The <see cref="IDebugUI_Builder"/> instance to draw it using</param>
        public void SetUpDebugUI(IDebugUI_Builder builder)
        {
            builder.StringField("State", GetStateName);
        }

        private string GetStateName()
        {
            if (CurrentState == null)
                return "None";

            return MiscUtils.ToDisplayString(CurrentState.GetType().Name);
        }
    }
}
