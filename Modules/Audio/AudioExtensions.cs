using UnityEngine;

namespace CookieUtils.Audio
{
    public static class AudioExtensions
    {
        public static void PlaySfx(this AudioClip clip, MonoBehaviour m, float volume = 1f, bool directional = true) => 
            AudioManager.PlaySound(clip, volume, m.transform.position, directional);
    }
}