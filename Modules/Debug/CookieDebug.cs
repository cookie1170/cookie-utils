using UnityEngine;
using UnityEngine.InputSystem;

namespace CookieUtils.CookieDebug
{
    public static class CookieDebug
    {
        public static bool IsDebugMode = false;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            var debugAction = new InputAction(binding: Keyboard.current.f3Key.path);
            debugAction.Enable();
            debugAction.performed += ToggleDebugMode;
        }

        private static void ToggleDebugMode(InputAction.CallbackContext ctx)
        {
            IsDebugMode = !IsDebugMode;
            Debug.Log($"Setting debug mode to {IsDebugMode}");
        }
    }
}
