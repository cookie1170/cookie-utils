using JetBrains.Annotations;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    [PublicAPI]
    public interface IDebugUI_If : IDebugUI_Group
    {
        /// <summary>
        ///     Starts an else group
        /// </summary>
        /// <returns>The <see cref="IDebugUI_Builder" /> instance to chain methods</returns>
        public IDebugUI_Group ElseGroup();
    }
}