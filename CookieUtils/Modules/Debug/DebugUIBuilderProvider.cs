using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils
{
    internal class DebugUIBuilderProvider : IDebugUIBuilderProvider
    {
        public IDebugUIBuilder Get(GameObject host)
        {
            return new DebugUIBuilder(host);
        }
    }
    
    [PublicAPI]
    public interface IDebugUIBuilderProvider
    {
        public IDebugUIBuilder Get(GameObject host);
        public IDebugUIBuilder Get(MonoBehaviour host) => Get(host.gameObject);

    }
}