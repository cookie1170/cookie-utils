using PrimeTween;
using UnityEngine;

namespace CookieUtils.Extras.Effect
{
    [CreateAssetMenu(fileName = "EffectData", menuName = "Cookie Utils/Effect data")]
    public class EffectData : ScriptableObject
    {
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
        public Effect.MaterialType materialType;
    }
}
