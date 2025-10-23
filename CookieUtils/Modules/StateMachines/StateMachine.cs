using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CookieUtils.StateMachines
{
    internal interface IStateMachine
    {
        void Update();
        void FixedUpdate();
    }

    [PublicAPI]
    public class StateMachine<T> : IStateMachine, IDisposable
    {
        private readonly T _host;

        private readonly Dictionary<Type, State<T>> _states = new();

        private bool _hasLinkedObject;
        private Object _linkedObject;

        public StateMachine(T host, Type defaultState, params State<T>[] states)
        {
            StateMachineUpdater.Register(this);

            _host = host;

            foreach (State<T> state in states) {
                _states.Add(state.GetType(), state);
                state.Host = host;
                state.Init(this);
            }

            ChangeState(defaultState);

            foreach (State<T> state in states) state.Start();
        }

        public State<T> CurrentState { get; private set; }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Called automatically during PlayerLoop.Update
        /// </summary>
        public void Update()
        {
            if (_hasLinkedObject && !_linkedObject) {
                Dispose();
                return;
            }

            CurrentState.Update();
        }

        /// <summary>
        ///     Called automatically during PlayerLoop.FixedUpdate
        /// </summary>
        public void FixedUpdate()
        {
            if (_hasLinkedObject && !_linkedObject) {
                Dispose();
                return;
            }

            CurrentState.FixedUpdate();
        }

        private string GetHostName()
        {
            return (_host as MonoBehaviour)?.name ?? typeof(T).Name;
        }

        public void ChangeState(Type state, bool keepObjectActive = false)
        {
            if (!_states.TryGetValue(state, out State<T> newState)) {
                Debug.LogWarning(
                    $"State machine under owner {(_host as MonoBehaviour)?.name ?? typeof(T).Name} does not include state of type {state}");
                return;
            }

            CurrentState?.Leave();
            CurrentState?.gameObject?.SetActive(keepObjectActive);

            newState.gameObject?.SetActive(true);
            newState.Enter();
            CurrentState = newState;
        }

        public void ChangeState<TState>(bool keepObjectActive = false)
        {
            ChangeState(typeof(TState), keepObjectActive);
        }

        ~StateMachine()
        {
            ReleaseUnmanagedResources();
        }

        private void ReleaseUnmanagedResources()
        {
            StateMachineUpdater.Deregister(this);
        }

        public StateMachine<T> AddTo(Object linkedObject)
        {
            _hasLinkedObject = true;
            _linkedObject = linkedObject;

            return this;
        }
    }
}