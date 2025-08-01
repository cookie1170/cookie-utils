using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CookieUtils.Runtime.Health
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class HurtEffect : MonoBehaviour
    {
        private static readonly int ProgressID = Shader.PropertyToID("_Progress");
        private static readonly int ColorID = Shader.PropertyToID("_Color");
        
        [SerializeField] private Color color = Color.crimson;
        [SerializeField, Foldout("Duration")] private float inTime = 0.15f;
        [SerializeField, Foldout("Duration")] private float holdTime = 0.1f;
        [SerializeField, Foldout("Duration")] private float outTime = 0.3f;
        [SerializeField, Foldout("Easing")] private Ease inEasing = Ease.InCirc;
        [SerializeField, Foldout("Easing")] private Ease outEasing = Ease.OutCirc;
        
        [SerializeField, HideInInspector] private Material hitMaterial;
        
        private SpriteRenderer _sprite;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _sprite.material = hitMaterial;
            _sprite.material.SetColor(ColorID, color);
        }

        public void OnHit()
        {
            Sequence s = DOTween.Sequence();
            s.Append(TweenProgress(1f, inTime).SetEase(inEasing));
            s.AppendInterval(holdTime);
            s.Append(TweenProgress(0f, outTime).SetEase(outEasing));
        }

        private Tweener TweenProgress(float to, float duration)
        {
            return DOVirtual.Float(_sprite.material.GetFloat(ProgressID), to, duration,
                v => _sprite.material.SetFloat(ProgressID, v));
        }
    }
}