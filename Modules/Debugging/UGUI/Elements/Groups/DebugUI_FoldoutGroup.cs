using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_FoldoutGroup : DebugUI_Group
    {
        [Header("References")]
        [SerializeField]
        private Sprite closedSprite;

        [SerializeField]
        private Sprite openSprite;

        [SerializeField]
        private DebugUI_InteractListener listener;

        [SerializeField]
        private Image arrow;

        [SerializeField]
        private TMP_Text textObject;
        private Func<string> _updateText;
        private bool _value;

        private void Awake()
        {
            listener.onClick.AddListener(OnClicked);
        }

        protected override void OnLateUpdate()
        {
            textObject.text = _updateText();
            Content.gameObject.SetActive(_value);
        }

        internal override void AddChild(GameObject child)
        {
            child.transform.SetParent(Content, false);
        }

        private void OnClicked()
        {
            _value = !_value;
            arrow.sprite = _value ? openSprite : closedSprite;
        }

        internal void Init(Func<string> updateText, bool defaultShown)
        {
            _value = defaultShown;
            _updateText = updateText;
        }
    }
}
