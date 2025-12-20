using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Group : DebugUI_Element
    {
        private RectTransform _content;

        protected internal RectTransform Content
        {
            get
            {
                if (!_content)
                    _content = transform.Find("Content").transform as RectTransform;

                return _content;
            }
        }

        internal virtual void AddChild(GameObject child)
        {
            child.transform.SetParent(Content, false);
        }
    }
}
