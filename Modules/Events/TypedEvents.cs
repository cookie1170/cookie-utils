using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.Events
{
    [PublicAPI]
    [CreateAssetMenu(menuName = "Cookie Utils/Float event", fileName = "Event", order = 0)]
    public class FloatEvent : Event<float> { }

    [PublicAPI]
    [CreateAssetMenu(menuName = "Cookie Utils/Int event", fileName = "Event", order = 0)]
    public class IntEvent : Event<int> { }

    [PublicAPI]
    [CreateAssetMenu(menuName = "Cookie Utils/Object event", fileName = "Event", order = 0)]
    public class ObjectEvent : Event<Object> { }
}
