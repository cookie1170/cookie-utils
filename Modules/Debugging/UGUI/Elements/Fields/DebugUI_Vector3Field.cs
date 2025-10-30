using System;
using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Vector3Field : DebugUI_Field
    {
        internal void Init(
            string text,
            Func<Vector3> updateValue,
            [CanBeNull] Action<Vector3> onValueEdited,
            string xLabel,
            string yLabel,
            string zLabel
        ) {
            label.text = text;
        }
    }
}