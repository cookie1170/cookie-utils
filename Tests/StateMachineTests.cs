using System;
using CookieUtils.StateMachines;
using NUnit.Framework;
using UnityEngine;

namespace CookieUtils.Tests
{
    public class StateMachineTests
    {
        [Test]
        public void ChangeStateTest()
        {
            bool didAEnter = false;
            bool didBEnter = false;
            bool didALeave = false;
            bool didBLeave = false;
            bool didAStart = false;
            bool didBStart = false;
            bool didAUpdate = false;
            bool didBUpdate = false;
            bool didAFixedUpdate = false;
            bool didBFixedUpdate = false;
            GameObject aObject = new();
            GameObject bObject = new();

            State<StateMachineTests>[] states =
            {
                new TestStateA(
                    () => didAEnter = true,
                    () => didALeave = true,
                    () => didAUpdate = true,
                    () => didAFixedUpdate = true,
                    () => didAStart = true
                )
                {
                    gameObject = aObject,
                },
                new TestStateB(
                    () => didBEnter = true,
                    () => didBLeave = true,
                    () => didBUpdate = true,
                    () => didBFixedUpdate = true,
                    () => didBStart = true
                )
                {
                    gameObject = bObject,
                },
            };
            StateMachine<StateMachineTests> stateMachine = new(this, typeof(TestStateA), states);

            Assert.IsTrue(didAStart);
            Assert.IsTrue(didBStart);

            Assert.IsTrue(didAEnter);
            Assert.IsFalse(didBEnter);
            Assert.IsTrue(aObject.activeSelf);
            Assert.IsFalse(bObject.activeSelf);

            stateMachine.Update();
            Assert.IsTrue(didAUpdate);
            Assert.IsFalse(didBUpdate);

            stateMachine.FixedUpdate();
            Assert.IsTrue(didAFixedUpdate);
            Assert.IsFalse(didBFixedUpdate);

            stateMachine.ChangeState<TestStateB>();
            Assert.IsTrue(didALeave);
            Assert.IsTrue(didBEnter);
            Assert.IsFalse(aObject.activeSelf);
            Assert.IsTrue(bObject.activeSelf);

            didAUpdate = false;

            stateMachine.Update();
            Assert.IsFalse(didAUpdate);
            Assert.IsTrue(didBUpdate);

            didAFixedUpdate = false;
            stateMachine.FixedUpdate();
            Assert.IsFalse(didAFixedUpdate);
            Assert.IsTrue(didBFixedUpdate);

            didAEnter = false;
            stateMachine.ChangeState<TestStateA>(true);

            Assert.IsTrue(didAEnter);
            Assert.IsTrue(didBLeave);
            Assert.IsTrue(aObject.activeSelf);
            Assert.IsTrue(bObject.activeSelf);
        }

        private class TestStateA : State<StateMachineTests>
        {
            private readonly Action _onEnter;
            private readonly Action _onFixedUpdate;
            private readonly Action _onLeave;
            private readonly Action _onStart;
            private readonly Action _onUpdate;

            public TestStateA(
                Action onEnter,
                Action onLeave,
                Action onUpdate,
                Action onFixedUpdate,
                Action onStart
            )
            {
                _onEnter = onEnter;
                _onLeave = onLeave;
                _onUpdate = onUpdate;
                _onFixedUpdate = onFixedUpdate;
                _onStart = onStart;
            }

            public override void Enter()
            {
                _onEnter();
            }

            public override void Leave()
            {
                _onLeave();
            }

            public override void FixedUpdate()
            {
                _onFixedUpdate();
            }

            public override void Update()
            {
                _onUpdate();
            }

            public override void Start()
            {
                _onStart();
            }
        }

        private class TestStateB : TestStateA
        {
            public TestStateB(
                Action onEnter,
                Action onLeave,
                Action onUpdate,
                Action onFixedUpdate,
                Action onStart
            )
                : base(onEnter, onLeave, onUpdate, onFixedUpdate, onStart) { }
        }
    }
}
