using UnityEngine;

namespace CookieUtils.Runtime.Audio
{
    public static class AudioExtensions
    {
        public static void PlaySfx(this MonoBehaviour m, AudioClip clip, float volume = 1f, bool directional = true) => 
            AudioManager.Inst.PlaySound(clip, volume, m.transform.position, directional);
    }
}