using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.Events
{
    // really hacky but i can't think of a better way
    [PublicAPI]
    [CreateAssetMenu(menuName = "Cookie Utils/Event", fileName = "Event", order = 0)]
    public class Event : ScriptableObject
    {
        protected readonly List<(Action method, CancellationToken token)> Methods = new();

        public void Subscribe(Action method)
        {
            var token = GetToken(method.Target);

            Methods.Add((method, token));
        }

        public void Unsubscribe(Action method)
        {
            int index = Methods.FindIndex(m => m.method == method);
            
            if (index != -1)
                Methods.RemoveAt(index);
        }

        public void Invoke()
        {
            for (int i = Methods.Count - 1; i >= 0; i--) {
                var method = Methods[i];
                if (method.token.IsCancellationRequested) {
                    Methods.RemoveAt(i);
                    continue;
                }

                method.method?.Invoke();
            }
        }
        
        
        internal static CancellationToken GetToken(object target)
        {
            CancellationToken token;

            if (target is MonoBehaviour behaviour) {
                token = behaviour.destroyCancellationToken;
            } else
                token = CancellationToken.None;

            return token;
        }
    }
    
    [PublicAPI]
    public class Event<T> : ScriptableObject
    {
        protected readonly List<(Action<T> method, CancellationToken token)> Methods = new();

        public void Subscribe(Action<T> method)
        {
            var token = Event.GetToken(method.Target);

            Methods.Add((method, token));
        }

        public void Unsubscribe(Action<T> method)
        {
            int index = Methods.FindIndex(m => m.method == method);
            
            if (index != -1)
                Methods.RemoveAt(index);
        }

        public void Invoke(T argument)
        {
            for (int i = Methods.Count - 1; i >= 0; i--) {
                var method = Methods[i];
                if (method.token.IsCancellationRequested) {
                    Methods.RemoveAt(i);
                    continue;
                }

                method.method?.Invoke(argument);
            }
        }
    }
}
