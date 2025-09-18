using System;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace CookieUtils.Extras.HurtEffect
{
    public class HurtEffect : MonoBehaviour
    {
        private static readonly int ProgressID = Shader.PropertyToID("_Progress");
        private static readonly int ColorID = Shader.PropertyToID("_Color");

        #region Serialized fields

        [Tooltip("Whether to use a data object")]
        public bool useDataObject;
        
        [Tooltip("The data object used for this hurt effect")]
        public HurtEffectData data;

        
        [Tooltip("Whether to shake the camera using CinemachineImpulseSource")]
        public bool shakeCamera;

        [Range(0, 2), Tooltip("The force to shake the camera with")]
        public float shakeForce = 0.25f;

        
        [Tooltip("Whether to punch the scale")]
        public bool animateScale = true;

        [Tooltip("The settings for the scale punch")]
        public ShakeSettings scaleSettings = new() {
            duration = 0.4f,
            strength = Vector3.one * 0.25f,
            enableFalloff = true,
            frequency = 2f,
            asymmetry = 0.5f,
        };
        
        [Tooltip("Whether to shake the rotation")]
        public bool animateRotation = true;

        [Tooltip("The settings to use for the rotation shake")]
        public ShakeSettings rotationSettings = new() {
            duration = 0.4f,
            enableFalloff = true,
            strength = Vector3.forward * 30f,
            frequency = 10f,
            asymmetry = 0.5f
        };
        
        [Tooltip("Whether to flash the sprite, requires a Renderer")]
        public bool animateFlash = true;

        [Tooltip("The tween settings for the flash fade in")]
        public TweenSettings<float> flashInSettings = new() {
            endValue = 1,
            startFromCurrent = true,
            settings = new() {
                ease = Ease.OutCirc,
                duration = 0.15f,
                endDelay = 0.2f
            }
        };

        [Tooltip("The tween settings for the flash fade in")]
        public TweenSettings<float> flashOutSettings = new() {
            endValue = 0,
            startFromCurrent = true,
            settings = new() {
                ease = Ease.InCirc,
                duration = 0.3f,
            }
        };
        
        [Tooltip("The color to flash to")]
        public Color flashColour = Color.crimson;
        
        [Tooltip("The type of material to use for the flash")]
        public MaterialType materialType;
        
        [Tooltip("Whether to override the renderer")]
        public bool overrideRenderer = false;

        [Tooltip("The renderer override")]
        public Renderer rendererOverride;
        
        [Tooltip("Whether to override the material, must have a _Color and a _Progress properties")]
        public bool overrideMaterial = false;
        
        [Tooltip("The material override, must have a _Color and a _Progress properties")]
        public Material materialOverride;
        
        #endregion
        
        #region Private fields
        
        [SerializeField, HideInInspector] private Material hitMaterialSpriteLit;
        [SerializeField, HideInInspector] private Material hitMaterialSpriteUnlit;
        [SerializeField, HideInInspector] private Material hitMaterialMeshLit;
        [SerializeField, HideInInspector] private Material hitMaterialMeshUnlit;

        private CinemachineImpulseSource _source;
        private Renderer _renderer;
        private Health.Health _health;
        
        #endregion
        
        #region Initialization

        private void Awake()
        {
            if (useDataObject && data) SetData();
            
            _health = GetComponentInParent<Health.Health>();

            if (!_health && transform.parent) {
                _health = transform.parent.GetComponentInChildren<Health.Health>();
            }

            if (!_health) {
                Debug.LogError($"No Health component attached to {(transform.parent ? transform.parent.name : name)}");
                Destroy(this);
                return;
            }

            if (shakeCamera) _source = GetComponent<CinemachineImpulseSource>();
            
            if (overrideRenderer && rendererOverride)
                _renderer = rendererOverride;
            else
                _renderer = GetComponent<Renderer>();
            
            if (!(_renderer && animateFlash)) return;
            
            _renderer.material = overrideMaterial && materialOverride
                ? materialOverride
                : materialType switch {
                    MaterialType.SpriteUnlit => hitMaterialSpriteUnlit,
                    MaterialType.SpriteLit => hitMaterialSpriteLit,
                    MaterialType.MeshUnlit => hitMaterialMeshUnlit,
                    MaterialType.MeshLit => hitMaterialMeshLit,
                    _ => throw new ArgumentOutOfRangeException(nameof(materialType))
                };
            
            _renderer.material.SetColor(ColorID, flashColour);
        }

        private void SetData()
        {
            shakeCamera = data.shakeCamera;
            shakeForce = data.shakeForce;
            animateScale = data.animateScale;
            scaleSettings = data.scaleSettings;
            animateRotation = data.animateRotation;
            rotationSettings = data.rotationSettings;
            animateFlash = data.animateFlash;
            flashInSettings = data.flashInSettings;
            flashOutSettings = data.flashOutSettings;
            flashColour = data.flashColour;
            materialType = data.materialType;
        }

        #endregion

        #region Effect

        public void OnHit(Health.Health.HitInfo info)
        {
            PrimeTweenConfig.warnEndValueEqualsCurrent = false;
            
            if (shakeCamera)
                ShakeCamera(info.Direction);

            if (animateScale)
                ShakeScale();

            if (animateRotation)
                ShakeRotation();

            if (animateFlash)
                Flash();
            
            PrimeTweenConfig.warnEndValueEqualsCurrent = true;
        }

        private void Flash()
        {
            Sequence.Create()
                .Chain(Tween.MaterialProperty(_renderer.material, ProgressID, flashInSettings))
                .Chain(Tween.MaterialProperty(_renderer.material, ProgressID, flashOutSettings));
        }

        private void ShakeRotation()
        {
            Tween.ShakeLocalRotation(transform, rotationSettings);
        }
        
        private void ShakeScale()
        {
            Tween.PunchScale(transform, scaleSettings);
        }

        private void ShakeCamera(Vector3 direction)
        {
            _source.GenerateImpulse(direction * shakeForce);
        }
        
        #endregion

        private void OnEnable()
        {
            _health.onHit.AddListener(OnHit);
        }
        
        private void OnDisable()
        {
            _health.onHit.RemoveListener(OnHit);
        }

        public enum MaterialType
        {
            SpriteLit,
            SpriteUnlit,
            MeshLit,
            MeshUnlit
        }
    }
}