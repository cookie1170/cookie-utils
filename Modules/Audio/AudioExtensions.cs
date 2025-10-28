using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.Audio
{
    [PublicAPI]
    public static class AudioExtensions
    {
        public static void Play(this AudioClip clip, Vector3 pos, float volume = 1f, float spatialBlend = 0) =>
            AudioManager.PlaySound(clip, volume, pos, spatialBlend);

        public static void Play(this AudioClip[] clips, Vector3 pos, float volume = 1f, float spatialBlend = 0) =>
            AudioManager.PlaySound(clips.Random(), volume, pos, spatialBlend);
    }
}