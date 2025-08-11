using DG.Tweening;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;

namespace CookieUtils.Runtime.Health
{
    public class HurtEffect : MonoBehaviour
    {
        private static readonly int ProgressID = Shader.PropertyToID("_Progress");
        private static readonly int ColorID = Shader.PropertyToID("_Color");

        [SerializeField, Foldout("Shake")] private bool shakeCamera;
        [SerializeField, Foldout("Shake"), ShowIf("shakeCamera")] private float force = 1.0f;
        
        [SerializeField, Foldout("Scale")] private bool tweenScale = true;
        [SerializeField, Foldout("Scale"), ShowIf("tweenScale")] private float scaleDuration = 0.2f;
        [SerializeField, Foldout("Scale"), ShowIf("tweenScale")] private float scaleAmount = 0.5f;
        [SerializeField, Foldout("Scale"), ShowIf("tweenScale")] private float scaleElasticity = 0.5f;
        [SerializeField, Foldout("Scale"), ShowIf("tweenScale")] private int scaleVibrato = 10;
        
        [SerializeField, Foldout("Rotation")] private bool tweenRotation = true;
        [SerializeField, Foldout("Rotation"), ShowIf("tweenRotation")] private float rotationDuration = 0.25f;
        [SerializeField, Foldout("Rotation"), ShowIf("tweenRotation")] private float rotationStrength = 60f;
        [SerializeField, Foldout("Rotation"), ShowIf("tweenRotation")] private float rotationRandomness = 30f;
        [SerializeField, Foldout("Rotation"), ShowIf("tweenRotation")] private int rotationVibrato = 5;
        
        [SerializeField, Foldout("Flash")] private bool tweenFlash = true;
        [SerializeField, Foldout("Flash"), ShowIf("tweenFlash")] private float fadeAmount = 0.8f;
        [SerializeField, Foldout("Flash"), ShowIf("tweenFlash")] private float inTime = 0.1f;
        [SerializeField, Foldout("Flash"), ShowIf("tweenFlash")] private float holdTime = 0.2f;
        [SerializeField, Foldout("Flash"), ShowIf("tweenFlash")] private float outTime = 0.3f;
        [SerializeField, Foldout("Flash"), ShowIf("tweenFlash")] private Ease inEasing = Ease.OutCirc;
        [SerializeField, Foldout("Flash"), ShowIf("tweenFlash")] private Ease outEasing = Ease.InCirc;
        [SerializeField, Foldout("Flash"), ShowIf("tweenFlash")] private Color color = Color.crimson;
        
        [SerializeField, HideInInspector] private Material hitMaterial;

        private CinemachineImpulseSource _impulseSource;
        private SpriteRenderer _sprite;
        private Sequence _currentFlash;
        private Vector3 _originalScale;
        private Quaternion _originalRotation;

        private void Awake()
        {
            if (shakeCamera) _impulseSource = GetComponent<CinemachineImpulseSource>();
            _sprite = GetComponent<SpriteRenderer>();
            if (!_sprite) return;
            _sprite.material = hitMaterial;
            _sprite.material.SetColor(ColorID, color);
            _originalScale = transform.localScale;
            _originalRotation = transform.rotation;
        }

        private void OnValidate()
        {
            if (shakeCamera)
            {
                if (!TryGetComponent(out CinemachineImpulseSource source))
                    source = gameObject.AddComponent<CinemachineImpulseSource>();

                _impulseSource = source;

                _impulseSource.ImpulseDefinition = new()
                {
                    ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Bump,
                    ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform
                };
            }
            
            else if (TryGetComponent(out CinemachineImpulseSource source))
                DestroyImmediate(source);
        }

        public void OnHit(Vector2 dir)
        {
            if (tweenFlash)
            {
                if (_currentFlash?.IsActive() ?? false)
                    _currentFlash.Kill();

                Sequence s = DOTween.Sequence();
                s.Append(_sprite.material.DOFloat(fadeAmount, ProgressID, inTime)).SetEase(inEasing);
                s.AppendInterval(holdTime);
                s.Append(_sprite.material.DOFloat(0f, ProgressID, outTime).SetEase(outEasing));
                _currentFlash = s;
            }

            if (tweenScale)
            {
                transform.DOPunchScale(Vector3.one * scaleAmount, scaleDuration, scaleVibrato, scaleElasticity)
                    .OnComplete(() => transform.localScale = _originalScale);
            }

            if (tweenRotation)
            {
                transform.DOShakeRotation(rotationDuration, rotationStrength, rotationVibrato, rotationRandomness)
                    .OnComplete(() => transform.rotation = _originalRotation);
            }

            if (shakeCamera)
            {
                _impulseSource.GenerateImpulseAt(transform.position, dir * force);
            }
        }
    }
}