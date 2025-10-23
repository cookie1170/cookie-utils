using CookieUtils;
using NUnit.Framework;
using UnityEngine;

public class BaseTests
{
    [Test]
    public void Vector2Math()
    {
        Vector2 vecWith = new(1, 2);
        Assert.AreEqual(new Vector2(2, 2), vecWith.With(x: 2));

        Vector2 vecAdd = new(3, 4);
        Assert.AreEqual(new Vector2(3, 6), vecAdd.Add(y: 2f));
    }

    [Test]
    public void Vector3Math()
    {
        Vector3 vecWith = new(1, 2, 3);
        Assert.AreEqual(new Vector3(1, 2, 2), vecWith.With(z: 2, x: 1));

        Vector3 vecAdd = new(3, 4, 6);
        Assert.AreEqual(new Vector3(4f, 6f, 6f), vecAdd.Add(y: 2f, x: 1f));
    }

    [Test]
    public void Swizzling()
    {
        Vector3 testVec = new(1, 2, 3);
        Assert.AreEqual(new Vector2(1, 3), testVec.xz());
        testVec.xz(3, 4);
        Assert.AreEqual(new Vector3(3, 2, 4), testVec);
    }

    [Test ]
    public void Remap()
    {
        const float sourceMin = 10;
        const float sourceMax = 20;
        const float targetMin = 30;
        const float targetMax = 50;
        const float v = 15;
        float result = CookieMath.Remap(v, sourceMin, sourceMax, targetMin, targetMax);

        Assert.AreEqual(40, result);
    }
}
