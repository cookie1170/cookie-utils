using CookieUtils.Timers;
using NUnit.Framework;
using UnityEngine;

namespace CookieUtils.Tests
{
    public class TimerTests
    {
        [Test]
        public void TickTest()
        {
            bool didPass = false;

            CountdownTimer timer = new(Time.deltaTime * 2) { OnComplete = () => didPass = true };
            timer.Update();
            Assert.AreEqual(Time.deltaTime * 2, timer.CurrentTime);
            timer.Start();
            timer.Update();
            Assert.AreEqual(Time.deltaTime * 2 - Time.deltaTime, timer.CurrentTime);
            timer.Update();
            Assert.IsTrue(didPass);
            timer.Dispose();
        }
    }
}
