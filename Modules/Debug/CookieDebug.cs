using UnityEngine;
using UnityEngine.InputSystem;

namespace CookieUtils
{
    /// <summary>
    /// A static class with methods and properties for debugging
    /// </summary>
    public static class CookieDebug
    {
        /// <summary>
        /// Is debug mode (toggled with F3) active
        /// </summary>
        public static bool IsDebugMode = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var debugAction = new InputAction(binding: Keyboard.current.f3Key.path);
            debugAction.Enable();
            debugAction.performed += ToggleDebugMode;
        }

        private static void ToggleDebugMode(InputAction.CallbackContext ctx)
        {
            IsDebugMode = !IsDebugMode;
            Debug.Log($"CookieUtils: Setting debug mode to {IsDebugMode}");
        }

        /// <summary>
        /// Draws a label in world position
        /// </summary>
        /// <param name="labelText">The text used for the label</param>
        /// <param name="worldPos">The world position of the label</param>
        public static void DrawLabelWorld(string labelText, Vector3 worldPos)
        {
            var rectPos = ToGUIPos(worldPos);
            GUI.Label(rectPos, labelText);
        }

        public static Rect ToGUIPos(Vector3 worldPos, Vector2? rect = null)
        {
            rect ??= new Vector2(100, 50);
            rect *= 0.5f;
            worldPos.y *= -1; // negative y for up is used in IMGui
            var labelPos = Camera.main.WorldToScreenPoint(worldPos);
            var rectPos = Rect.MinMaxRect(labelPos.x - rect.Value.x, labelPos.y - rect.Value.y,
                labelPos.x + rect.Value.x, labelPos.y + rect.Value.y);
            return rectPos;
        }
    }
}
