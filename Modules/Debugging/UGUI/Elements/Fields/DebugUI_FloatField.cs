using System;
using JetBrains.Annotations;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_FloatField : DebugUI_TextField
    {
        internal void Init(string text, Func<float> updateValue, [CanBeNull] Action<float> onValueEdited) {
            label.text = text;
        }
    }
}