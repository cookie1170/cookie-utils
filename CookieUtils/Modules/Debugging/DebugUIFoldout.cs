using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils.Debugging
{
    internal class DebugUIFoldout : MonoBehaviour
    {
        [SerializeField] private Sprite closedSprite;
        [SerializeField] private Sprite openSprite;
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image arrow;
        [SerializeField] private TMP_Text textObject;
        public Transform content;
        
        private void Awake()
        {
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool value)
        {
            arrow.sprite = value ? openSprite : closedSprite;
        }

        private void LateUpdate()
        {
            for (int i = 0; i < content.childCount; i++) {
                var child = content.GetChild(i).gameObject;
                child.SetActive(toggle.isOn);
            }
        }

        public void SetText(string text)
        {
            textObject.text = text;
        }
    }
}
