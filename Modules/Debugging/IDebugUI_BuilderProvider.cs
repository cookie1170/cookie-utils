using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.Debugging
{
    /// <summary>
    ///     Provides an instance of <see cref="IDebugUI_Builder" /> using <see cref="GetFor(GameObject)" />
    /// </summary>
    [PublicAPI]
    // ReSharper disable once InconsistentNaming
    public interface IDebugUI_BuilderProvider
    {
        /// <summary>
        ///     Provides the <see cref="IDebugUI_Builder" />
        /// </summary>
        /// <param name="host">The host to use for the builder, used for positioning</param>
        /// <returns>An instance of <see cref="IDebugUI_Builder" /></returns>
        public IDebugUI_Builder GetFor(GameObject host);

        /// <inheritdoc cref="GetFor(GameObject)" />
        public IDebugUI_Builder GetFor(MonoBehaviour host) => host ? GetFor(host.gameObject) : GetFor((GameObject)null);
    }
}