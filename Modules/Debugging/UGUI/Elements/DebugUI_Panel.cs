using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Panel : DebugUI_Group
    {
        [SerializeField]
        private Image lockIcon;

        [SerializeField]
        private Sprite lockedSprite;

        [SerializeField]
        private Sprite unlockedSprite;

        [SerializeField]
        internal Button lockButton;

        private Image _imageCached;

        internal Image Image
        {
            get
            {
                if (_imageCached)
                    return _imageCached;

                _imageCached = GetComponent<Image>();

                return _imageCached;
            }
        }

        internal void SetLocked(bool value)
        {
            lockIcon.sprite = value ? lockedSprite : unlockedSprite;
        }
    }
}
