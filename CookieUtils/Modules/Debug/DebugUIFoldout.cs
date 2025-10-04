using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils
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
            for (int i = 0; i < content.childCount; i++) {
                var child = content.GetChild(i).gameObject;
                Debug.Log($"Setting {child}'s active to {value}");
                child.SetActive(value);
            }

            arrow.sprite = value ? openSprite : closedSprite;
        }

        public void SetText(string text)
        {
            textObject.text = text;
        }
    }
}
