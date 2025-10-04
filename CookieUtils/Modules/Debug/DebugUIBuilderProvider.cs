using System;
using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils
{
    internal class DebugUIBuilderProvider : IDebugUIBuilderProvider
    {
        internal DebugUIBuilderProvider()
        {
            
        }        
        
        public IDebugUIBuilder Get(GameObject host)
        {
            return new DebugUIBuilder(host);
        }
    }

    internal class DummyDebugUIBuilderProvider : IDebugUIBuilderProvider
    {
        public IDebugUIBuilder Get(GameObject host)
        {
            return new DummyDebugUIBuilder(host);
        }
    }
    
    [PublicAPI]
    public interface IDebugUIBuilderProvider
    {
        public IDebugUIBuilder Get(GameObject host);

        public IDebugUIBuilder Get(MonoBehaviour host)
        {
            return Get(host.gameObject);
        }
    }
}