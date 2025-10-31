using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_FoldoutGroup : DebugUI_Group
    {
        [Header("References")] [SerializeField]
        private Sprite closedSprite;

        [SerializeField] private Sprite openSprite;
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image arrow;
        [SerializeField] private TMP_Text textObject;
        private Func<string> _updateText;

        private void Awake() {
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnLateUpdate() {
            textObject.text = _updateText();
            Content.gameObject.SetActive(toggle.isOn);
        }

        protected override void OnDestroyed() {
            toggle.onValueChanged.RemoveAllListeners();
        }

        internal override void AddChild(GameObject child) {
            child.transform.SetParent(Content, false);
        }

        private void OnValueChanged(bool value) {
            arrow.sprite = value ? openSprite : closedSprite;
        }

        internal void Init(Func<string> updateText, bool defaultShown) {
            toggle.isOn = defaultShown;
            _updateText = updateText;
        }
    }
}