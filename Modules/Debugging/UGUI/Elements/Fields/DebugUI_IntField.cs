using System;
using JetBrains.Annotations;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_IntField : DebugUI_TextField
    {
        internal void Init(string text, Func<int> updateValue, [CanBeNull] Action<int> onValueEdited) {
            label.text = text;
        }
    }
}