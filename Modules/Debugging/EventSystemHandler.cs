using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace CookieUtils.Debugging
{
    internal class EventSystemHandler : MonoBehaviour
    {
        private void Awake()
        {
            CookieDebug.DebugModeChanged += OnDebugModeChanged;
        }

        private void OnDestroy()
        {
            CookieDebug.DebugModeChanged -= OnDebugModeChanged;
        }

        public void OnDebugModeChanged(bool value)
        {
            bool anyOther = FindObjectsByType<EventSystem>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None
                )
                .Any(ev => ev.gameObject != gameObject && ev.isActiveAndEnabled);
            if (anyOther)
            {
                Destroy(gameObject);
                return;
            }

            if (!TryGetComponent<EventSystem>(out _))
            {
                gameObject.AddComponent<EventSystem>();
                gameObject.AddComponent<InputSystemUIInputModule>();
            }

            gameObject.SetActive(value);
        }
    }
}
