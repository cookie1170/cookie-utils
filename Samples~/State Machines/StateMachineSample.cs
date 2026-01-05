using System;
using CookieUtils.StateMachines;
using UnityEngine;

namespace CookieUtils.Samples.StateMachines
{
    public class StateMachineSample : MonoBehaviour
    {
        private StateMachine<StateMachineSample> _stateMachine;

        private void Awake()
        {
            State<StateMachineSample>[] states =
            {
                new StateRight(),
                new StateLeft(),
                new StateDown(),
                new StateUp(),
            };

            _stateMachine = new StateMachine<StateMachineSample>(this, typeof(StateRight), states);
        }

        private void OnGUI()
        {
            GUILayout.Label(_stateMachine.GetStateName());

            if (GUILayout.Button("Right"))
                _stateMachine.Get<StateRight>().Enter();

            if (GUILayout.Button("Left"))
                _stateMachine.Get<StateLeft>().Enter();

            if (GUILayout.Button("Down"))
                _stateMachine.Get<StateDown>().Enter();

            if (GUILayout.Button("Up"))
                _stateMachine.Get<StateUp>().Enter();
        }
    }

    public enum EnterReason
    {
        Button,
        Edge,
    }

    [Serializable]
    public class StateRight : State<StateMachineSample, EnterReason>
    {
        protected override EnterReason DefaultData => EnterReason.Button;

        protected override void OnEnter(EnterReason reason)
        {
            Debug.Log($"Moving right for reason {reason}");
        }

        protected override void OnFixedUpdate()
        {
            float speed = LastData == EnterReason.Edge ? 4 : 2;
            Vector3 pos = Host.transform.position;
            pos.x += Time.deltaTime * speed;
            Host.transform.position = pos;

            if (pos.x > Camera.main.orthographicSize * Camera.main.aspect)
            {
                Get<StateLeft>().Enter(EnterReason.Edge);
            }
        }
    }

    [Serializable]
    public class StateLeft : State<StateMachineSample, EnterReason>
    {
        protected override EnterReason DefaultData => EnterReason.Button;

        protected override void OnEnter(EnterReason reason)
        {
            Debug.Log($"Moving left for reason {reason}");
        }

        protected override void OnFixedUpdate()
        {
            float speed = LastData == EnterReason.Edge ? 4 : 2;
            Vector3 pos = Host.transform.position;
            pos.x -= Time.deltaTime * speed;
            Host.transform.position = pos;

            if (pos.x < -Camera.main.orthographicSize * Camera.main.aspect)
            {
                Get<StateRight>().Enter(EnterReason.Edge);
            }
        }
    }

    [Serializable]
    public class StateUp : State<StateMachineSample, EnterReason>
    {
        protected override EnterReason DefaultData => EnterReason.Button;

        protected override void OnEnter(EnterReason reason)
        {
            Debug.Log($"Moving up for reason {reason}");
        }

        protected override void OnFixedUpdate()
        {
            float speed = LastData == EnterReason.Edge ? 4 : 2;
            Vector3 pos = Host.transform.position;
            pos.y += Time.deltaTime * speed;
            Host.transform.position = pos;

            if (pos.y > Camera.main.orthographicSize)
            {
                Get<StateDown>().Enter(EnterReason.Edge);
            }
        }
    }

    [Serializable]
    public class StateDown : State<StateMachineSample, EnterReason>
    {
        protected override EnterReason DefaultData => EnterReason.Button;

        protected override void OnEnter(EnterReason reason)
        {
            Debug.Log($"Moving down for reason {reason}");
        }

        protected override void OnFixedUpdate()
        {
            float speed = LastData == EnterReason.Edge ? 4 : 2;
            Vector3 pos = Host.transform.position;
            pos.y -= Time.deltaTime * speed;
            Host.transform.position = pos;

            if (pos.y < -Camera.main.orthographicSize)
            {
                Get<StateUp>().Enter(EnterReason.Edge);
            }
        }
    }
}
