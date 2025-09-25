using System;
using CookieUtils.Audio;
using CookieUtils.Runtime.ObjectPooling;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
// ReSharper disable MemberCanBeProtected.Global

namespace CookieUtils.Extras.Juice
{
    public class Effect : MonoBehaviour
    {
        private static readonly int ProgressID = Shader.PropertyToID("_Progress");
        private static readonly int ColorID = Shader.PropertyToID("_Color");

        #region Serialized fields

        [Tooltip("The data object used for this effect")]
        public EffectData data;

        [Tooltip("Whether to override the renderers")]
        public bool overrideRenderers = false;

        [Tooltip("The renderer overrides")] public Renderer[] rendererOverrides;

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
        private Renderer[] _renderers;
        private Transform _cam;

        #endregion

        #region Initialization

        protected virtual void Awake()
        {
            var mainCam = Camera.main;
            Debug.Assert(mainCam != null, "Camera.main != null");
            _cam = mainCam.transform;

            if (!data) {
                Debug.LogError($"{(transform.parent ? transform.parent.name : name)}'s Effect has no data object!");
                Destroy(this);
                return;
            }

            if (data.shakeCamera) {
                if (!TryGetComponent(out _source)) {
                    _source = gameObject.AddComponent<CinemachineImpulseSource>();
                }
            }

            if (overrideRenderers && rendererOverrides.Length > 0)
                _renderers = rendererOverrides;
            else
                _renderers = GetComponentsInChildren<Renderer>();

            if (!(_renderers.Length > 0 && data.animateFlash)) return;

            foreach (var rendererIteration in _renderers) {
                rendererIteration.material = overrideMaterial && materialOverride
                    ? materialOverride
                    : data.materialType switch {
                        MaterialType.Lit => data.is2D ? hitMaterialSpriteLit : hitMaterialMeshLit,
                        MaterialType.Unlit => data.is2D ? hitMaterialSpriteUnlit : hitMaterialMeshUnlit,
                        _ => throw new ArgumentOutOfRangeException(nameof(data.materialType))
                    };

                rendererIteration.material.SetColor(ColorID, data.flashColour);
            }
        }

        #endregion

        #region Effect
        
        public virtual void Play()
        {
            var difference = transform.position - _cam.position;
            if (data.is2D) difference.z = 0;
            

            var direction = difference.normalized;

            if (direction.sqrMagnitude < 0.05f * 0.05f) direction = Vector3.right;

            Play(direction);
        }

        public virtual void Play(Vector3 direction)
        {
            PrimeTweenConfig.warnEndValueEqualsCurrent = false;

            if (data.shakeCamera)
                ShakeCamera(direction);

            if (data.spawnParticles)
                SpawnParticles(direction);

            if (data.playAudio)
                PlayAudio();
                
            if (data.animateScale)
                AnimateScale();

            if (data.animateRotation)
                AnimateRotation();

            if (data.animateFlash)
                AnimateFlash();

            PrimeTweenConfig.warnEndValueEqualsCurrent = true;
        }

        private void ShakeCamera(Vector3 direction)
        {
            if (!_source) _source = GetComponent<CinemachineImpulseSource>();

            _source.GenerateImpulse(direction * data.shakeForce);
        }

        private void SpawnParticles(Vector3 direction)
        {
            var angle = Quaternion.identity;

            if (data.directionalParticles) {
                angle = data.is2D
                    ? Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, direction))
                    : Quaternion.AngleAxis(0, direction);
            }
            
            data.particlePrefab.Get(transform.position, angle);
        }

        private async void PlayAudio()
        {
            await Awaitable.WaitForSecondsAsync(data.audioDelay, destroyCancellationToken);
         
            var pos = transform.position;
            
            data.audioClips.Play(pos, data.audioVolume, data.spatialBlend);
        }

        private void AnimateScale()
        {
            var sequence = Sequence.Create();
            foreach (var instruction in data.scaleAnimation) {
                var tween = instruction.Process(transform);
                if (instruction.parallel) sequence.Group(tween);
                else sequence.Chain(tween);
            }
        }

        private void AnimateRotation()
        {
            var sequence = Sequence.Create();
            foreach (var instruction in data.rotationAnimation) {
                var tween = instruction.Process(transform);
                if (instruction.parallel) sequence.Group(tween);
                else sequence.Chain(tween);
            }
        }

        private void AnimateFlash()
        {
            foreach (var rendererIteration in _renderers) {
                var sequence = Sequence.Create();

                for (int i = 0; i < data.flashAnimation.Length; i++) {
                    var instruction = data.flashAnimation[i];
                    var settings = instruction.settings;
                    if (i != 0) settings.startFromCurrent = true;
                    
                    var tween = Tween.MaterialProperty(rendererIteration.material, ProgressID, settings);
                    
                    if (instruction.parallel) sequence.Group(tween);
                    else sequence.Chain(tween);
                }
            }
        }

        #endregion

        public enum MaterialType
        {
            Lit,
            Unlit
        }
    }
}