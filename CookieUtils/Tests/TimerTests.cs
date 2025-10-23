using System.Collections;
using CookieUtils.Timers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TimerTests
{
    [Test]
    public void TickTest()
    {
        bool didPass = false;

        CountdownTimer timer = new(Time.deltaTime * 2) {
            OnComplete = () => didPass = true
        };
        timer.Tick();
        Assert.AreEqual(Time.deltaTime * 2, timer.CurrentTime);
        timer.Start();
        timer.Tick();
        Assert.AreEqual(Time.deltaTime * 2 - Time.deltaTime, timer.CurrentTime);
        timer.Tick();
        Assert.IsTrue(didPass);
        timer.Dispose();
    }

    [UnityTest]
    public IEnumerator DestroyTest()
    {
        bool didComplete = false;
        TestBehaviour testBehaviour = new GameObject().AddComponent<TestBehaviour>();

        CountdownTimer timer = new(Time.deltaTime - 0.005f) {
            OnComplete = () => didComplete = true
        };
        timer.AddTo(testBehaviour);

        timer.Start();

        Object.DestroyImmediate(testBehaviour.gameObject);
        yield return null;

        timer.Tick();

        Assert.IsFalse(didComplete);
    }

    internal class TestBehaviour : MonoBehaviour
    {
    }
}