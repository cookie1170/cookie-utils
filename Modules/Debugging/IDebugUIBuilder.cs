using System;
using JetBrains.Annotations;

namespace CookieUtils.Debugging
{
    /// <summary>
    ///     Used for creating debug UIs
    /// </summary>
    /// <seealso cref="IDebugDrawer" />
    [PublicAPI]
    public interface IDebugUIBuilder
    {
        /// <summary>
        ///     Draws a label
        /// </summary>
        /// <param name="updateText">Returns the text to display on the label. Called every update</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder Label(Func<string> updateText);

        /// <summary>
        ///     Draws a label
        /// </summary>
        /// <param name="text">The text to display on the label</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder Label(string text) => Label(() => text);

        /// <summary>
        ///     Starts a foldout group
        /// </summary>
        /// <param name="updateText">Returns the text to display next to the foldout. Called every update</param>
        /// <param name="defaultShown">Whether the foldout is visible at the start</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder FoldoutGroup(Func<string> updateText, bool defaultShown = true);

        /// <summary>
        ///     Starts a foldout group
        /// </summary>
        /// <param name="text">The text displayed next to the foldout</param>
        /// <param name="defaultShown">Whether the foldout is visible at the start</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder FoldoutGroup(string text, bool defaultShown = true) =>
            FoldoutGroup(() => text, defaultShown);

        /// <summary>
        ///     Starts an if group, which will only be shown if <c>condition</c> evaluates to <c>true</c>
        /// </summary>
        /// <param name="condition">The condition to check for</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder IfGroup(Func<bool> condition);

        /// <summary>
        ///     Starts an else group, which will only be shown if the previous <see cref="IfGroup" /> isn't
        /// </summary>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder ElseGroup();

        /// <summary>
        ///     Starts a switch group, which should contain children <see cref="CaseGroup{T}(Func{T})">CaseGroup</see>s
        /// </summary>
        /// <param name="condition">The condition to evaluate</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder SwitchGroup<T>(Func<T> condition);

        /// <summary>
        ///     Starts a case group, which is only shown if the parent <see cref="SwitchGroup{T}">SwitchGroup</see>'s
        ///     <c>condition</c> evaluates to true
        /// </summary>
        /// <param name="value">
        ///     The value that the parent <see cref="SwitchGroup{T}">SwitchGroup</see>'s <c>condition</c> should
        ///     evaluate to
        /// </param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder CaseGroup<T>(Func<T> value);

        /// <inheritdoc cref="CaseGroup{T}(Func{T})" />
        public IDebugUIBuilder CaseGroup<T>(T value) => CaseGroup(() => value);

        /// <summary>
        ///     Ends the current group
        /// </summary>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder EndGroup();
    }
}