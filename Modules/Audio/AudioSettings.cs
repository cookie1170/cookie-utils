using System.Linq;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CookieUtils.Audio
{
    [SettingsObject("Audio", "Cookie Utils/Audio", "Audio", "Sound", "Volume")]
    public class AudioSettings : SettingsObject<AudioSettings>
    {
        private const string defaultOutputPath =
            "Packages/com.cookie.cookieutils/Modules/Audio/Mixer.mixer";
        private const string sfxGroup = "Sfx";

        public AudioManager.AudioConfig defaultConfig = new()
        {
            delay = 0,
            volume = 1f,
            minVolumeMult = 0.9f,
            maxVolumeMult = 1f,
            pitch = 1f,
            minPitchMult = 0.8f,
            maxPitchMult = 1.2f,
            spatialBlend = 0,
            minDistance = 1,
            maxDistance = 500,
            bypassEffects = false,
            bypassReverbZones = false,
            outputGroup = null,
        };

#if UNITY_EDITOR
        private void Reset()
        {
            if (defaultConfig.outputGroup == null)
                defaultConfig.outputGroup = AssetDatabase
                    .LoadAssetAtPath<AudioMixer>(defaultOutputPath)
                    .FindMatchingGroups(sfxGroup)
                    .FirstOrDefault();
        }

#endif

#if UNITY_EDITOR
        [SettingsProvider]
        public static SettingsProvider ProvideSettings() => GetSettings();
#endif
    }
}
