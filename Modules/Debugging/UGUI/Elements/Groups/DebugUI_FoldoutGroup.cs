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

        private void Update() {
            textObject.text = _updateText();
        }

        private void LateUpdate() {
            Content.gameObject.SetActive(toggle.isOn);
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