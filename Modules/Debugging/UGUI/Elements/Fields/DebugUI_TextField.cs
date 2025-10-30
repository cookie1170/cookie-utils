using TMPro;
using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal abstract class DebugUI_TextField : DebugUI_Field
    {
        [SerializeField] protected TMP_InputField inputField;
    }
}