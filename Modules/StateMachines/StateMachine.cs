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

            foreach (State<T> state in states)
                state.Start();

            Get(defaultState).Enter();
        }

        public TState Get<TState>()
            where TState : State<T> => (TState)Get(typeof(TState));

        public State<T> Get(Type defaultState)
        {
            if (!_states.ContainsKey(defaultState))
            {
                throw new ArgumentException(
                    $"State machine under host {GetHostName()} doesn't contain a state of type {defaultState.Name}"
                );
            }

            return _states[defaultState];
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

        internal void ChangeState(State<T> newState, bool keepObjectActive = false)
        {
            CurrentState?.Leave(keepObjectActive);

            CurrentState = newState;
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
            foreach (State<T> state in _states.Values)
            {
                IDebugUI_If ifState = builder.IfGroup(() => CurrentState == state);
                state.SetUpDebugUIInternal(ifState);
            }
        }

        public string GetStateName()
        {
            if (CurrentState == null)
                return "None";

            return MiscUtils.ToDisplayString(CurrentState.GetType().Name);
        }
    }
}

/*
StateMachine.Get<MyState>().Enter(data?);

class MyState : State<Host, Data> {

}

abstract class State<THost, TData> : State<THost> {
    public void new Enter(TData data) {
        StateMachine.ChangeState(this);
        OnEnter(data);
    }

    protected virtual void new OnEnter(TData data) { }
}

abstract class State<THost> {
    public void Enter() {
        StateMachine.ChangeState(this);
        OnEnter()
    }

    protected virtual void OnEnter() { }
}
*/
