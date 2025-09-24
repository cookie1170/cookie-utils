using System;
using CookieUtils.Audio;
using CookieUtils.Runtime.ObjectPooling;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace CookieUtils.Extras.Juice
{
    public class Effect : MonoBehaviour
    {
        private static readonly int ProgressID = Shader.PropertyToID("_Progress");
        private static readonly int ColorID = Shader.PropertyToID("_Color");

        #region Serialized fields

        [Tooltip("Whether to use a data object")]
        public bool useDataObject;

        [Tooltip("The data object used for this effect")]
        public EffectData data;


        [Tooltip("Whether to use 2D coordinates for direction and particle rotation")]
        public bool is2D = true;
        
        
        [Tooltip("Whether to shake the camera using CinemachineImpulseSource")]
        public bool shakeCamera;

        [Range(0, 2), Tooltip("The force to shake the camera with")]
        public float shakeForce = 0.25f;


        [Tooltip("Whether to spawn particles")]
        public bool spawnParticles;

        [Tooltip("Whether the particles should be directional. Expected original direction is to the right in 2D and forward in 3D")]
        public bool directionalParticles;

        [Tooltip("The particles to spawn")]
        public GameObject particlePrefab;


        [Tooltip("Whether to play audio")]
        public bool playAudio = false;

        [Tooltip("The spatial blend of the sound"), Range(0, 1)]
        public float spatialBlend = 0f;
        
        [Tooltip("The volume of the sound"), Range(0, 1)]
        public float audioVolume = 1f;

        [Tooltip("The delay with which to pay the audio clip")]
        public float audioDelay = 0;
        
        [Tooltip("The audio clips to play (random pick)")]
        public AudioClip[] audioClips;
        

        [Tooltip("Whether to punch the scale")]
        public bool animateScale = true;

        [Tooltip("The animation for the scale")]
        public ScaleTweenInstruction[] scaleAnimation = {
            new() {
                type = TweenType.Punch,
                shakeSettings = new() {
                    duration = 0.4f,
                    strength = Vector3.one * 0.25f,
                    enableFalloff = true,
                    frequency = 2f,
                    asymmetry = 0.5f,
                }
            }
        };
        

        [Tooltip("Whether to shake the rotation")]
        public bool animateRotation = true;

        [Tooltip("The animation for the rotation")]
        public RotationTweenInstruction[] rotationAnimation = {
            new() {
                type = TweenType.Punch,
                shakeSettings = new() {
                    duration = 0.4f,
                    enableFalloff = true,
                    strength = Vector3.forward * 30f,
                    frequency = 10f,
                    asymmetry = 0.5f
                }
            }
        };
        

        [Tooltip("Whether to flash the sprite, requires a Renderer")]
        public bool animateFlash = true;

        [Tooltip("The animation for the flash (0 is original colour, 1 is flash colour)")]
        public FloatTweenInstruction[] flashAnimation = {
            new() {
                settings = new() {
                    endValue = 1,
                    settings = new() {
                        ease = Ease.OutCirc,
                        duration = 0.15f,
                        endDelay = 0.2f
                    }
                }
            },
            new() {
                settings = new() {
                    startValue = 1,
                    endValue = 0,
                    settings = new() {
                        ease = Ease.InCirc,
                        duration = 0.3f,
                    }
                }
            }
        };
        
        [Tooltip("The color to flash to")]
        public Color flashColour = Color.crimson;

        [Tooltip("The type of material to use for the flash")]
        public MaterialType materialType;

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

            if (useDataObject && data) UpdateData();

            if (shakeCamera) {
                if (!TryGetComponent(out _source)) {
                    _source = gameObject.AddComponent<CinemachineImpulseSource>();
                }
            }

            if (overrideRenderers && rendererOverrides.Length > 0)
                _renderers = rendererOverrides;
            else
                _renderers = GetComponentsInChildren<Renderer>();

            if (!(_renderers.Length > 0 && animateFlash)) return;

            foreach (var rendererIteration in _renderers) {
                rendererIteration.material = overrideMaterial && materialOverride
                    ? materialOverride
                    : materialType switch {
                        MaterialType.Lit => is2D ? hitMaterialSpriteLit : hitMaterialMeshLit,
                        MaterialType.Unlit => is2D ? hitMaterialSpriteUnlit : hitMaterialMeshUnlit,
                        _ => throw new ArgumentOutOfRangeException(nameof(materialType))
                    };

                rendererIteration.material.SetColor(ColorID, flashColour);
            }
        }

        public void UpdateData()
        {
            is2D = data.is2D;
            shakeCamera = data.shakeCamera;
            shakeForce = data.shakeForce;
            spawnParticles = data.spawnParticles;
            playAudio = data.playAudio;
            spatialBlend = data.spatialBlend;
            audioVolume = data.audioVolume;
            audioDelay = data.audioDelay;
            audioClips = data.audioClips;
            directionalParticles = data.directionalParticles;
            particlePrefab = data.particlePrefab;
            animateScale = data.animateScale;
            scaleAnimation = data.scaleAnimation;
            animateRotation = data.animateRotation;
            rotationAnimation = data.rotationAnimation;
            animateFlash = data.animateFlash;
            flashAnimation = data.flashAnimation;
            flashColour = data.flashColour;
            materialType = data.materialType;
        }

        #endregion

        #region Effect
        
        public virtual void Play()
        {
            var difference = transform.position - _cam.position;
            if (is2D) difference.z = 0;
            

            var direction = difference.normalized;

            if (direction.sqrMagnitude < 0.05f * 0.05f) direction = Vector3.right;

            Play(direction);
        }

        public virtual void Play(Vector3 direction)
        {
            PrimeTweenConfig.warnEndValueEqualsCurrent = false;

            if (shakeCamera)
                ShakeCamera(direction);

            if (spawnParticles)
                SpawnParticles(direction);

            if (playAudio)
                PlayAudio();
                
            if (animateScale)
                AnimateScale();

            if (animateRotation)
                AnimateRotation();

            if (animateFlash)
                AnimateFlash();

            PrimeTweenConfig.warnEndValueEqualsCurrent = true;
        }

        private void ShakeCamera(Vector3 direction)
        {
            if (!_source) _source = GetComponent<CinemachineImpulseSource>();

            _source.GenerateImpulse(direction * shakeForce);
        }

        private void SpawnParticles(Vector3 direction)
        {
            var angle = Quaternion.identity;

            if (directionalParticles) {
                angle = is2D
                    ? Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, direction))
                    : Quaternion.AngleAxis(0, direction);
            }
            
            particlePrefab.Get(transform.position, angle);
        }

        private async void PlayAudio()
        {
            await Awaitable.WaitForSecondsAsync(audioDelay, destroyCancellationToken);
         
            var pos = transform.position;
            
            audioClips.Play(pos, audioVolume, spatialBlend);
        }

        private void AnimateScale()
        {
            var sequence = Sequence.Create();
            foreach (var instruction in scaleAnimation) {
                var tween = instruction.Process(transform);
                if (instruction.parallel) sequence.Group(tween);
                else sequence.Chain(tween);
            }
        }

        private void AnimateRotation()
        {
            var sequence = Sequence.Create();
            foreach (var instruction in rotationAnimation) {
                var tween = instruction.Process(transform);
                if (instruction.parallel) sequence.Group(tween);
                else sequence.Chain(tween);
            }
        }

        private void AnimateFlash()
        {
            foreach (var rendererIteration in _renderers) {
                var sequence = Sequence.Create();

                for (int i = 0; i < flashAnimation.Length; i++) {
                    var instruction = flashAnimation[i];
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