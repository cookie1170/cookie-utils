using UnityEngine;

namespace CookieUtils.Debugging
{
    internal class UGUIDebugUIBuilderProvider : IDebugUIBuilderProvider
    {
        private readonly int _index;

        internal UGUIDebugUIBuilderProvider(int index) => _index = index;

        public IDebugUIBuilder GetFor(GameObject host) {
            if (host) return new UGUIDebugUIBuilder(host);

            CookieDebug.RegisteredObjects.RemoveAt(_index);
            return new DummyDebugUIBuilder();
        }
    }
}