using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace CookieUtils.Modules.Health
{
    public class HurtEffect : MonoBehaviour
    {
        private static readonly int ProgressID = Shader.PropertyToID("_Progress");
        private static readonly int ColorID = Shader.PropertyToID("_Color");
        
        public bool shakeCamera;
        public float force = 1.0f;
        
        public bool tweenScale = true;
        public float scaleDuration = 0.2f;
        public float scaleAmount = 0.5f;
        public float scaleElasticity = 0.5f;
        public int scaleVibrato = 10;
        
        public bool tweenRotation = true;
        public bool onlyZ = false;
        public float rotationDuration = 0.25f;
        public float rotationStrength = 60f;
        public float rotationRandomness = 30f;
        public int rotationVibrato = 5;
        
        public bool tweenFlash = true;
        public float fadeAmount = 0.8f;
        public float inTime = 0.1f;
        public float holdTime = 0.2f;
        public float outTime = 0.3f;
        public Ease inEasing = Ease.OutCirc;
        public Ease outEasing = Ease.InCirc;
        public Color color = Color.crimson;
        
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
                if (onlyZ)
                {
                    transform.DOShakeRotation(rotationDuration, Vector3.forward * rotationStrength, rotationVibrato, rotationRandomness)
                        .OnComplete(() => transform.rotation = _originalRotation);
                }
                else
                {
                    transform.DOShakeRotation(rotationDuration, rotationStrength, rotationVibrato, rotationRandomness)
                        .OnComplete(() => transform.rotation = _originalRotation);
                    
                }
            }

            if (shakeCamera)
            {
                _impulseSource.GenerateImpulseAt(transform.position, dir * force);
            }
        }
    }
}