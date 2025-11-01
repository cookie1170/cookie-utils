using UnityEngine;

namespace CookieUtils.Debugging
{
    internal class UGUIDebugUIBuilderProvider : IDebugUIBuilderProvider
    {
        public IDebugUIBuilder GetFor(GameObject host) {
            if (host) return new UGUIDebugUIBuilder(host);

            return new DummyDebugUIBuilder();
        }
    }
}