using System;
using JetBrains.Annotations;

namespace CookieUtils.Debugging
{
    [PublicAPI]
    // ReSharper disable once InconsistentNaming
    public interface IDebugUI_Switch
    {
        /// <summary>
        ///     Starts a case group, which is only shown if the parent
        ///     <see cref="IDebugUI_Builder.SwitchGroup{T}">SwitchGroup</see>'s
        ///     <c>condition</c> evaluates to true
        /// </summary>
        /// <param name="value">
        ///     The value that the parent <see cref="IDebugUI_Builder.SwitchGroup{T}">SwitchGroup</see>'s <c>condition</c> should
        ///     evaluate to
        /// </param>
        /// <returns>An <see cref="IDebugUI_Group" /> group</returns>
        public IDebugUI_Group CaseGroup<T>(Func<T> value);

        /// <inheritdoc cref="CaseGroup{T}(Func{T})" />
        public IDebugUI_Group CaseGroup<T>(T value) => CaseGroup(() => value);
    }
}