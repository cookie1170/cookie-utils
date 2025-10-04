using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils
{
    internal class DebugUIBuilder : IDebugUIBuilder
    {
        private readonly GameObject _host;
        private DebugUIPosition _position = DebugUIPosition.Top;
        private int _tabIndex;

        internal DebugUIBuilder(GameObject host)
        {
            _host = host;
        }
        
        public IDebugUIBuilder Label(string text)
        {
            return this;
        }

        public IDebugUIBuilder SetPosition(DebugUIPosition position)
        {
            _position = position;
            return this;
        }

        DebugUIOptions IDebugUIBuilder.GetOptions()
        {
            return new DebugUIOptions(_host, _position);
        }
    }

    internal class DummyDebugUIBuilder : IDebugUIBuilder
    {
        private DebugUIPosition _position;
        private readonly GameObject _hostObject;

        public DummyDebugUIBuilder(GameObject hostObject)
        {
            _hostObject = hostObject;
        }

        public IDebugUIBuilder SetPosition(DebugUIPosition position)
        {
            _position = position;
            return this;
        }

        DebugUIOptions IDebugUIBuilder.GetOptions()
        {
            return new DebugUIOptions(_hostObject, _position);
        }
    }
    
    
    [PublicAPI]
    public interface IDebugUIBuilder
    {
        public IDebugUIBuilder Label(string text) => this;
        public IDebugUIBuilder SetPosition(DebugUIPosition position) => this;
        internal DebugUIOptions GetOptions();
    }
}