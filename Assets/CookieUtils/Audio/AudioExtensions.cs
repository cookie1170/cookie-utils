using UnityEngine;

namespace CookieUtils.Audio
{
    public static class AudioExtensions
    {
        public static void PlaySfx(this MonoBehaviour _, AudioClip clip, Transform position, float volume = 1f) => 
            AudioManager.Inst.PlaySfx(clip, position, volume);
    }
}