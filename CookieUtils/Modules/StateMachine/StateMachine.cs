using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace CookieUtils.StateMachine
{
    internal interface IStateMachine
    {
        void Update();
        void FixedUpdate();
    }

    public class StateMachine<T> : IStateMachine, IDisposable where T : MonoBehaviour
    {
        private readonly T _host;
        private readonly CancellationToken _token;

        public State<T> CurrentState { get; private set; }

        private readonly Dictionary<Type, State<T>> _states = new();

        public StateMachine(T host, List<State<T>> states, Type defaultState)
        {
            StateMachineUpdater.Register(this);
            
            _host = host;
            _token = host.destroyCancellationToken;

            foreach (var state in states) {
                _states.Add(state.GetType(), state);
                state.Host = host;
                state.Init(this);
            }

            if (_states.TryGetValue(defaultState, out var defaultStateObj))
                CurrentState = defaultStateObj;
            else
                Debug.LogWarning($"Default state of type {defaultState} not included in states of object {host.name}");

            foreach (var state in states) state.Start();
        }

        public void ChangeState(Type state, bool keepObjectActive = false)
        {
            if (!_states.TryGetValue(state, out var newState)) {
                Debug.LogWarning($"State machine under owner {_host.name} does not include state of type {state}");
                return;
            }

            if (!_host) {
                return;
            }

            CurrentState.Leave();
            CurrentState.GameObject?.SetActive(keepObjectActive);

            newState.GameObject?.SetActive(true);
            newState.Enter();
            CurrentState = newState;
        }

        public void ChangeState<TState>(bool keepObjectActive = false)
        {
            ChangeState(typeof(TState), keepObjectActive);
        }

        /// <summary>
        /// Called automatically during PlayerLoop.Update
        /// </summary>
        public void Update()
        {
            if (!_host || _token.IsCancellationRequested) {
                Dispose();
                return;
            }
            CurrentState.Update();
        }

        /// <summary>
        /// Called automatically during PlayerLoop.FixedUpdate
        /// </summary>
        public void FixedUpdate()
        {
            if (!_host || _token.IsCancellationRequested) {
                Dispose();
                return;
            }
            CurrentState.FixedUpdate();
        }

        ~StateMachine()
        {
            ReleaseUnmanagedResources();
        }

        private void ReleaseUnmanagedResources()
        {
            StateMachineUpdater.Deregister(this);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
    }
}