using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils
{
    internal class DebugUIBuilder : IDebugUIBuilder
    {
        private readonly GameObject _host;
        private readonly VisualElement _root;
        private readonly Func<string, bool> _lookupGet;
        private readonly Action<string, bool> _lookupSet;
        private DebugUIPosition _position = DebugUIPosition.Top;
        private Foldout _currentFoldout;
        private int _tabIndex;

        internal DebugUIBuilder(GameObject host, Func<string, bool> lookupGet, Action<string, bool> lookupSet)
        {
            _host = host;
            _lookupGet = lookupGet;
            _lookupSet = lookupSet;
            _root = new VisualElement();
            _root.AddToClassList("panel");
        }

        private VisualElement GetParent()
        {
            return _currentFoldout ?? _root;
        }
        
        public IDebugUIBuilder Label(string text)
        {
            var label = new Label(text) {
                tabIndex = ++_tabIndex
            };
            GetParent().Add(label);
            return this;
        }

        public IDebugUIBuilder Foldout(string text)
        {
            string name = $"{text}_{_host.GetInstanceID()}";
            var foldout = new Foldout {
                text = text,
                name = name,
                value = _lookupGet(name),
                toggleOnLabelClick = true
            };

            GetParent().Add(foldout);
            _currentFoldout = foldout;
            return this;
        }

        public IDebugUIBuilder EndFoldout()
        {
            _currentFoldout = null;
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

        VisualElement IDebugUIBuilder.Build()
        {
            return _root;
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

        VisualElement IDebugUIBuilder.Build()
        {
            return new VisualElement();
        }
    }
    
    
    [PublicAPI]
    public interface IDebugUIBuilder
    {
        public IDebugUIBuilder Label(string text) => this;
        public IDebugUIBuilder Foldout(string text) => this;
        public IDebugUIBuilder EndFoldout() => this;
        public IDebugUIBuilder SetPosition(DebugUIPosition position) => this;

        internal DebugUIOptions GetOptions();
        internal VisualElement Build();
    }
}