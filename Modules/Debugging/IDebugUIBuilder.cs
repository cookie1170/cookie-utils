using System;
using JetBrains.Annotations;
using UnityEngine;

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
        ///     Draws a float field
        /// </summary>
        /// <param name="text">The text displayed next to the field</param>
        /// <param name="updateValue">Returns the value to display. Called every update</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder FloatField(string text, Func<float> updateValue) => FloatField(text, updateValue, null);

        /// <summary>
        ///     Draws an editable float field
        /// </summary>
        /// <param name="text">The text displayed next to the field</param>
        /// <param name="updateValue">Returns the value to display. Called every update</param>
        /// <param name="onValueEdited">Called when the value of the field is edited</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder FloatField(string text, Func<float> updateValue, Action<float> onValueEdited);

        /// <summary>
        ///     Draws an integer field
        /// </summary>
        /// <param name="text">The text displayed next to the field</param>
        /// <param name="updateValue">Returns the value to display. Called every update</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder IntField(string text, Func<int> updateValue) => IntField(text, updateValue, null);

        /// <summary>
        ///     Draws an editable integer field
        /// </summary>
        /// <param name="text">The text displayed next to the field</param>
        /// <param name="updateValue">Returns the value to display. Called every update</param>
        /// <param name="onValueEdited">Called when the value of the field is edited</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder IntField(string text, Func<int> updateValue, Action<int> onValueEdited);

        /// <summary>
        ///     Draws a boolean value field
        /// </summary>
        /// <param name="text">The text displayed next to the field</param>
        /// <param name="updateValue">Returns the value to display. Called every update</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder BoolField(string text, Func<bool> updateValue) => BoolField(text, updateValue, null);

        /// <summary>
        ///     Draws an editable boolean value field
        /// </summary>
        /// <param name="text">The text displayed next to the field</param>
        /// <param name="updateValue">Returns the value to display. Called every update</param>
        /// <param name="onValueEdited">Called when the value of the field is edited</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder BoolField(string text, Func<bool> updateValue, Action<bool> onValueEdited);

        /// <summary>
        ///     Draws a string field
        /// </summary>
        /// <param name="text">The text displayed next to the field</param>
        /// <param name="updateValue">Returns the value to display. Called every update</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder StringField(string text, Func<string> updateValue) =>
            StringField(text, updateValue, null);

        /// <summary>
        ///     Draws an editable string field
        /// </summary>
        /// <param name="text">The text displayed next to the field</param>
        /// <param name="updateValue">Returns the value to display. Called every update</param>
        /// <param name="onValueEdited">Called when the value of the field is edited</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder StringField(string text, Func<string> updateValue, Action<string> onValueEdited);


        /// <summary>
        ///     Draws a Vector2 field
        /// </summary>
        /// <param name="text">The text displayed next to the field</param>
        /// <param name="updateValue">Returns the value to display. Called every update</param>
        /// <param name="xLabel">The label for the <c>x</c> component of the vector</param>
        /// <param name="yLabel">The label for the <c>y</c> component of the vector</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder Vector2Field(
            string text,
            Func<Vector2> updateValue,
            string xLabel = "x",
            string yLabel = "y"
        ) => Vector2Field(text, updateValue, null, xLabel, yLabel);

        /// <summary>
        ///     Draws an editable Vector2 field
        /// </summary>
        /// <param name="text">The text displayed next to the field</param>
        /// <param name="updateValue">Returns the value to display. Called every update</param>
        /// <param name="onValueEdited">Called when the value of the field is edited</param>
        /// <param name="xLabel">The label for the <c>x</c> component of the vector</param>
        /// <param name="yLabel">The label for the <c>y</c> component of the vector</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder Vector2Field(
            string text,
            Func<Vector2> updateValue,
            Action<Vector2> onValueEdited,
            string xLabel = "x",
            string yLabel = "y"
        );

        /// <summary>
        ///     Draws a Vector3 field
        /// </summary>
        /// <param name="text">The text displayed next to the field</param>
        /// <param name="updateValue">Returns the value to display. Called every update</param>
        /// <param name="xLabel">The label for the <c>x</c> component of the vector</param>
        /// <param name="yLabel">The label for the <c>y</c> component of the vector</param>
        /// <param name="zLabel">The label for the <c>z</c> component of the vector</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder Vector3Field(
            string text,
            Func<Vector3> updateValue,
            string xLabel = "x",
            string yLabel = "y",
            string zLabel = "z"
        ) => Vector3Field(text, updateValue, null, xLabel, yLabel, zLabel);

        /// <summary>
        ///     Draws an editable Vector3 field
        /// </summary>
        /// <param name="text">The text displayed next to the field</param>
        /// <param name="updateValue">Returns the value to display. Called every update</param>
        /// <param name="onValueEdited">Called when the value of the field is edited</param>
        /// <param name="xLabel">The label for the <c>x</c> component of the vector</param>
        /// <param name="yLabel">The label for the <c>y</c> component of the vector</param>
        /// <param name="zLabel">The label for the <c>z</c> component of the vector</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder Vector3Field(
            string text,
            Func<Vector3> updateValue,
            Action<Vector3> onValueEdited,
            string xLabel = "x",
            string yLabel = "y",
            string zLabel = "z"
        );

        /// <summary>
        ///     Draws a button
        /// </summary>
        /// <param name="updateText">Returns the text to display on the button. Called every update</param>
        /// <param name="onClicked">Called when the button is clicked</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder Button(Func<string> updateText, Action onClicked);

        /// <summary>
        ///     Draws a button
        /// </summary>
        /// <param name="text">The text to display on the button</param>
        /// <param name="onClicked">Called when the button is clicked</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder Button(string text, Action onClicked) => Button(() => text, onClicked);

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