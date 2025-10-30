using System;
using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Vector2Field : DebugUI_Field
    {
        internal void Init(
            string text,
            Func<Vector2> updateValue,
            [CanBeNull] Action<Vector2> onValueEdited,
            string xLabel,
            string yLabel
        ) {
            label.text = text;
        }
    }
}