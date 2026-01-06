using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_InteractListener : Selectable, IPointerDownHandler
    {
        public UnityEvent onClick;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsInteractable())
                return;

            onClick?.Invoke();
        }
    }
}
