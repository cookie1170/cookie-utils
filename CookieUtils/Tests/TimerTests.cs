using System.Threading;
using CookieUtils.Timer;
using NUnit.Framework;
using UnityEngine;

public class TimerTests
{
    [Test]
    public void TickTest()
    {
        bool didPass = false;
        CountdownTimer timer = new(Time.deltaTime * 2, CancellationToken.None) {
            OnComplete = () => didPass = true 
        };
        timer.Tick();
        Assert.AreEqual(Time.deltaTime * 2, timer.CurrentTime);
        timer.Start();
        timer.Tick();
        Assert.AreEqual(Time.deltaTime * 2 - Time.deltaTime, timer.CurrentTime);
        timer.Tick();
        Assert.IsTrue(didPass);
    }

    [Test]
    public void DestroyTest()
    {
        bool didComplete = false;
        CancellationTokenSource tokenSource = new();

        CountdownTimer timer = new(Time.deltaTime - 0.005f, tokenSource.Token) {
            OnComplete = () => didComplete = true,
        };

        timer.Start();
        
        tokenSource.Cancel();

        timer.Tick();
        
        Assert.IsFalse(didComplete);
    }
}
