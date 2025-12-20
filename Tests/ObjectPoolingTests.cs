using CookieUtils.ObjectPooling;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CookieUtils.Tests
{
    public class ObjectPoolingTests
    {
        private static bool _wasGetCalled = false;
        private static bool _wasReleaseCalled = false;

        [Test]
        public void SameTest()
        {
            var prefab = new GameObject();

            GameObject objOne = prefab.Get();
            Assert.IsTrue(objOne.activeSelf);

            objOne.Release();
            Assert.IsFalse(objOne.activeSelf);

            GameObject objTwo = prefab.Get();
            Assert.IsTrue(objTwo.activeSelf);
            Assert.AreEqual(objOne, objTwo);

            Object.DestroyImmediate(objOne);
            Object.DestroyImmediate(prefab);
            Object.DestroyImmediate(PoolManager.Inst);
        }

        [Test]
        public void CallbackReceiverTest()
        {
            _wasGetCalled = false;
            _wasReleaseCalled = false;
            var prefab = new GameObject().AddComponent<CallbackReceiverTestBehaviour>();

            CallbackReceiverTestBehaviour behaviour = prefab.Get();
            Assert.IsTrue(_wasGetCalled);

            behaviour.Release();
            Assert.IsTrue(_wasReleaseCalled);
            Object.DestroyImmediate(prefab);
            Object.DestroyImmediate(behaviour);
            Object.DestroyImmediate(PoolManager.Inst.gameObject);
            _wasGetCalled = false;
            _wasReleaseCalled = false;
        }

        private class CallbackReceiverTestBehaviour : MonoBehaviour, IPoolCallbackReceiver
        {
            public void OnGet()
            {
                _wasGetCalled = true;
            }

            public void OnRelease()
            {
                _wasReleaseCalled = true;
            }
        }
    }
}
