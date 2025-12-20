using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Event = CookieUtils.Events.Event;
using Object = UnityEngine.Object;

namespace CookieUtils.Tests
{
    public class EventsTests
    {
        [Test]
        public void InvokeTest()
        {
            bool didInvoke = false;
            var testEvent = ScriptableObject.CreateInstance<Event>();

            testEvent.Subscribe(OnInvoke, testEvent);

            testEvent.Invoke();
            Assert.IsTrue(didInvoke);

            Object.DestroyImmediate(testEvent);

            return;

            void OnInvoke()
            {
                didInvoke = true;
            }
        }

        [Test]
        public void UnsubscribeTest()
        {
            bool didInvoke = false;
            var testEvent = ScriptableObject.CreateInstance<Event>();

            testEvent.Subscribe(OnInvoke, testEvent);
            testEvent.Unsubscribe(OnInvoke);
            testEvent.Invoke();

            Assert.IsFalse(didInvoke);

            Object.DestroyImmediate(testEvent);

            void OnInvoke()
            {
                didInvoke = true;
            }
        }

        [UnityTest]
        public IEnumerator MonoBehaviourTest()
        {
            var testEvent = ScriptableObject.CreateInstance<Event>();
            var testBehaviour = new GameObject().AddComponent<TestBehaviour>();

            testEvent.Subscribe(testBehaviour.OnInvoke, testBehaviour);
            testEvent.Invoke();
            Assert.IsTrue(testBehaviour.didInvoke);

            Object.DestroyImmediate(testEvent);
            Object.DestroyImmediate(testBehaviour);

            yield break;
        }

        [UnityTest]
        public IEnumerator DestroyTest()
        {
            var testEvent = ScriptableObject.CreateInstance<Event>();
            var testBehaviour = new GameObject().AddComponent<TestBehaviour>();

            testEvent.Subscribe(testBehaviour.OnInvoke, testBehaviour);
            Object.DestroyImmediate(testBehaviour);

            yield return null;
            try
            {
                testEvent.Invoke();
            }
            catch (NullReferenceException e)
            {
                Assert.Fail(e.Message);
            }

            if (
                testEvent
                    .GetType()
                    .GetRuntimeFields()
                    .First(field => field.Name.ToUpper() == "METHODS")
                    .GetValue(testEvent)
                is List<(Action method, CancellationToken token)> methods
            )
                Assert.AreEqual(0, methods.Count);

            Object.DestroyImmediate(testEvent);
            Object.DestroyImmediate(testBehaviour);
        }

        internal class TestBehaviour : MonoBehaviour
        {
            public bool didInvoke;

            public void OnInvoke()
            {
                didInvoke = true;
            }
        }
    }
}
