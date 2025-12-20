using JetBrains.Annotations;

namespace CookieUtils.Debugging
{
    /// <summary>
    ///     An if group for debug UI that is shown when its condition evaluates to true
    /// </summary>
    [PublicAPI]
    // ReSharper disable once InconsistentNaming
    public interface IDebugUI_If : IDebugUI_Group
    {
        /// <summary>
        ///     Starts an else group
        /// </summary>
        /// <returns>The <see cref="IDebugUI_Builder" /> instance to chain methods</returns>
        public IDebugUI_Group ElseGroup();
    }
}
