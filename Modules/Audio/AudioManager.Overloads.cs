using UnityEngine;

namespace CookieUtils.Audio
{
    public partial class AudioManager : Singleton<AudioManager>
    {
        public static void PlaySound(AudioClip clip, Vector3 position)
        {
            AudioConfig config = AudioSettings.Get().defaultConfig;
            Instance.PlaySoundInstance(clip, position, config);
        }

        public static void PlaySound(AudioClip clip, Vector3 position, float volume = 1f)
        {
            AudioConfig config = AudioSettings.Get().defaultConfig;
            config.volume = volume;
            Instance.PlaySoundInstance(clip, position, config);
        }

        public static void PlaySound(
            AudioClip clip,
            Vector3 position,
            float volume = 1f,
            float pitch = 1f
        )
        {
            AudioConfig config = AudioSettings.Get().defaultConfig;
            config.volume = volume;
            config.pitch = pitch;
            Instance.PlaySoundInstance(clip, position, config);
        }
    }
}
