using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.Debugging
{
    internal class DebugUIBuilderProvider : IDebugUIBuilderProvider
    {
        public IDebugUIBuilder Get(GameObject host)
        {
            return new DebugUIBuilder(host);
        }
    }
    
    /// <summary>
    /// Provides an instance of <see cref="IDebugUIBuilder"/> using <see cref="Get(GameObject)"/>
    /// </summary>
    [PublicAPI]
    public interface IDebugUIBuilderProvider
    {
        /// <summary>
        /// Provides the <see cref="IDebugUIBuilder"/>
        /// </summary>
        /// <param name="host">The host to use for the builder, used for positioning</param>
        /// <returns>An instance of <see cref="IDebugUIBuilder"/></returns>
        public IDebugUIBuilder Get(GameObject host);

        /// <inheritdoc cref="Get(UnityEngine.GameObject)" />
        public IDebugUIBuilder Get(MonoBehaviour host) => Get(host.gameObject);

    }
}