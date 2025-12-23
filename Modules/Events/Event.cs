using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CookieUtils.Events
{
    // really hacky but i can't think of a better way
    [PublicAPI]
    [CreateAssetMenu(menuName = "Cookie Utils/Event", fileName = "Event", order = 0)]
    public class Event : ScriptableObject
    {
        protected readonly List<(Action method, Object host)> Methods = new();

        public void Subscribe(Action method, Object host)
        {
            Methods.Add((method, host));
        }

        public void Unsubscribe(Action method)
        {
            int index = Methods.FindIndex(m => m.method == method);

            if (index != -1)
                Methods.RemoveAt(index);
        }

        public void Invoke()
        {
            for (int i = Methods.Count - 1; i >= 0; i--)
            {
                (Action method, Object host) method = Methods[i];
                if (!method.host)
                {
                    Methods.RemoveAt(i);

                    continue;
                }

                method.method?.Invoke();
            }
        }
    }

    [PublicAPI]
    public class Event<T> : ScriptableObject
    {
        protected readonly List<(Action<T> method, Object host)> Methods = new();

        public void Subscribe(Action<T> method, Object host)
        {
            Methods.Add((method, host));
        }

        public void Unsubscribe(Action<T> method)
        {
            int index = Methods.FindIndex(m => m.method == method);

            if (index != -1)
                Methods.RemoveAt(index);
        }

        public void Invoke(T argument)
        {
            for (int i = Methods.Count - 1; i >= 0; i--)
            {
                (Action<T> method, Object host) method = Methods[i];
                if (!method.host)
                {
                    Methods.RemoveAt(i);

                    continue;
                }

                method.method?.Invoke(argument);
            }
        }
    }
}
