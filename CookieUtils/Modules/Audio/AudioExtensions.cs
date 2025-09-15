using UnityEngine;

namespace CookieUtils.Audio
{
    public static class AudioExtensions
    {
        public static void PlaySfx(this AudioClip clip, Vector2 pos, float volume = 1f, bool directional = true) => 
            AudioManager.PlaySound(clip, volume, pos, directional);
    }
}