using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal abstract class DebugUI_Group : DebugUI_Element
    {
        private RectTransform _content;

        protected RectTransform Content {
            get {
                if (!_content) _content = transform.Find("Content").transform as RectTransform;

                return _content;
            }
        }

        internal virtual void AddChild(GameObject child) {
            child.transform.SetParent(Content, false);
        }
    }
}