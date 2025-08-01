using System;
using System.Collections.Generic;
using UnityEngine;

namespace CookieUtils.StateMachine
{
	public class StateMachine<T> where T : MonoBehaviour
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
				state.Init(this);
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

		public void ChangeState(Type state, bool keepObjectActive = false)
		{
			if (!states.TryGetValue(state, out State<T> newState))
			{
				Debug.LogWarning($"State machine under owner {host.name} does not include state of type {state}");
				return;
			}

			if (!host)
			{
				Debug.Log("State machine destroyed!");
				return;
			}

			// Debug.Log($"Changing {host.GetType().Name}'s state to {state.Name}");
			
			CurrentState.Leave();
			// CurrentState.GameObject?.SetActive(keepObjectActive);
			
			// newState.GameObject?.SetActive(true);
			newState.Enter();
			CurrentState = newState;
		}

		public void Update()
		{
			if (!host) return;
			CurrentState.Update();
		}

		public void FixedUpdate()
		{
			if (!host) return;
			CurrentState.FixedUpdate();
		}
	}
}