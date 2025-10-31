using TMPro;
using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal abstract class DebugUI_Field : DebugUI_Element
    {
        [SerializeField] protected TMP_Text label;
    }
}