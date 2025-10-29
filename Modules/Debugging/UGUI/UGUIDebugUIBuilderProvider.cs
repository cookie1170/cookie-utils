using UnityEngine;

namespace CookieUtils.Debugging
{
    internal class UGUIDebugUIBuilderProvider : IDebugUIBuilderProvider
    {
        public IDebugUIBuilder GetFor(GameObject host) => new UGUIDebugUIBuilder(host);
    }
}