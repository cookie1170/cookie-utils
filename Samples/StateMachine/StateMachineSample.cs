using System;
using System.Collections.Generic;
using UnityEngine;
using CookieUtils.StateMachines;

namespace Samples.StateMachine
{
    public class StateMachineSample : MonoBehaviour
    {
        private StateMachine<StateMachineSample> _stateMachine;
        
        private void Awake()
        {
            var states = new State<StateMachineSample>[]
            {
                new StateRight(),
                new StateLeft(),
                new StateDown(),
                new StateUp()
            };

            _stateMachine = new(this, typeof(StateRight), states);
        }

        private void OnGUI()
        {
            GUILayout.Label(_stateMachine.CurrentState.GetType().Name);
            
            if (GUILayout.Button("Right"))
                _stateMachine.ChangeState<StateRight>();
            
            if (GUILayout.Button("Left"))
                _stateMachine.ChangeState<StateLeft>();
            
            if (GUILayout.Button("Down"))
                _stateMachine.ChangeState<StateDown>();
            
            if (GUILayout.Button("Up"))
                _stateMachine.ChangeState<StateUp>();
        }
    }
    
    [Serializable]
    public class StateRight : State<StateMachineSample>
    {
        public override void FixedUpdate()
        {
            Vector3 pos = Host.transform.position;
            pos.x += Time.deltaTime * 2;
            Host.transform.position = pos;
        }
    }
    
    [Serializable]
    public class StateLeft : State<StateMachineSample>
    {
        public override void FixedUpdate()
        {
            Vector3 pos = Host.transform.position;
            pos.x -= Time.deltaTime * 2;
            Host.transform.position = pos;
        }
    }
    
    [Serializable]
    public class StateUp : State<StateMachineSample>
    {
        public override void FixedUpdate()
        {
            Vector3 pos = Host.transform.position;
            pos.y += Time.deltaTime * 2;
            Host.transform.position = pos;
        }
    }
    
    [Serializable]
    public class StateDown : State<StateMachineSample>
    {
        public override void FixedUpdate()
        {
            Vector3 pos = Host.transform.position;
            pos.y -= Time.deltaTime * 2;
            Host.transform.position = pos;
        }
    }
}
