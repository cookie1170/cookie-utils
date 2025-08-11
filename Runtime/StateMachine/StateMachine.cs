using System;
using System.Collections.Generic;
using UnityEngine;

namespace CookieUtils.StateMachine
{
	public class StateMachine<T> where T : MonoBehaviour
	{
		private readonly T _host;
		
		public State<T> CurrentState { get; private set; }

		private readonly Dictionary<Type, State<T>> _states = new();
		
		public StateMachine(T host, List<State<T>> states, Type defaultState)
		{
			_host = host;

			foreach (State<T> state in states)
			{
				_states.Add(state.GetType(), state);
				state.Host = host;
				state.Init(this);
			}
			
			if (_states.TryGetValue(defaultState, out State<T> defaultStateObj))
			{
				CurrentState = defaultStateObj;
			}
			else
			{
				Debug.LogWarning($"Default state of type {defaultState} not included in states of object {host.name}");
			}

			foreach (State<T> state in states)
			{
				state.Start();
			}
		}

		public void ChangeState(Type state, bool keepObjectActive = false)
		{
			if (!_states.TryGetValue(state, out State<T> newState))
			{
				Debug.LogWarning($"State machine under owner {_host.name} does not include state of type {state}");
				return;
			}

			if (!_host)
			{
				Debug.Log("State machine destroyed!");
				return;
			}
			
			CurrentState.Leave();
			// CurrentState.GameObject?.SetActive(keepObjectActive);
			
			// newState.GameObject?.SetActive(true);
			newState.Enter();
			CurrentState = newState;
		}

		public void ChangeState<TState>(bool keepObjectActive)
		{
			ChangeState(typeof(TState), keepObjectActive);
		}

		public void Update()
		{
			if (!_host) return;
			CurrentState.Update();
		}

		public void FixedUpdate()
		{
			if (!_host) return;
			CurrentState.FixedUpdate();
		}
	}
}