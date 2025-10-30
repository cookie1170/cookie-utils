using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.Debugging
{
    /// <summary>
    ///     Provides an instance of <see cref="IDebugUIBuilder" /> using <see cref="GetFor(GameObject)" />
    /// </summary>
    [PublicAPI]
    public interface IDebugUIBuilderProvider
    {
        /// <summary>
        ///     Provides the <see cref="IDebugUIBuilder" />
        /// </summary>
        /// <param name="host">The host to use for the builder, used for positioning</param>
        /// <returns>An instance of <see cref="IDebugUIBuilder" /></returns>
        public IDebugUIBuilder GetFor(GameObject host);

        /// <inheritdoc cref="GetFor(GameObject)" />
        public IDebugUIBuilder GetFor(MonoBehaviour host) { if (host) return GetFor(host.gameObject); return GetFor((GameObject)null);  }
    }
}