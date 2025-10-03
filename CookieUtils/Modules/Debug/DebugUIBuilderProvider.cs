using System;
using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils
{
    internal class DebugUIBuilderProvider : IDebugUIBuilderProvider
    {
        private readonly Func<string, bool> _lookupGet;
        private readonly Action<string, bool> _lookupSet;
        
        internal DebugUIBuilderProvider(Func<string, bool> lookupGet, Action<string, bool> lookupSet)
        {
            _lookupGet = lookupGet;
            _lookupSet = lookupSet;
        }        
        
        public IDebugUIBuilder Get(GameObject host)
        {
            return new DebugUIBuilder(host, _lookupGet, _lookupSet);
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