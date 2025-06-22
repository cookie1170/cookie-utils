using System;
using System.Collections.Generic;
using UnityEngine;

namespace CookieUtils.StateMachine
{
	public class StateMachine<T>
	{
		private T host;
		
		public State<T> CurrentState { get; private set; }

		private readonly Dictionary<Type, State<T>> states = new();
		
		public StateMachine(T host, List<State<T>> states, Type defaultState)
		{
			this.host = host;

			foreach (State<T> state in states)
			{
				this.states.Add(state.GetType(), state);
				state.Host = host;
				state.StateMachine = this;
			}
			
			if (this.states.TryGetValue(defaultState, out State<T> defaultStateObj))
			{
				CurrentState = defaultStateObj;
			}
			else
			{
				Debug.LogWarning($"Default state of type {defaultState} not included in states of object {host}");
			}
		}

		public void ChangeState(Type state)
		{
			if (!states.TryGetValue(state, out State<T> newState))
			{
				Debug.LogWarning($"State machine under owner {host} does not include state of type {state}");
				return;
			}
			
			CurrentState.Leave();
			CurrentState = newState;
			newState.Enter();
		}

		public void Update() => CurrentState.Update();

		public void FixedUpdate() => CurrentState.FixedUpdate();
	}
}