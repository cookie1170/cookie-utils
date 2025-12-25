using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.Audio
{
    [PublicAPI]
    public static class AudioExtensions
    {
        public static void Play(this AudioClip clip, Vector3 pos) =>
            AudioManager.PlaySound(clip, pos);

        public static void Play(this AudioClip[] clips, Vector3 pos) =>
            AudioManager.PlaySound(clips.Random(), pos);

        public static void Play(this AudioClip clip, Vector3 pos, float volume = 1f) =>
            AudioManager.PlaySound(clip, pos, volume);

        public static void Play(this AudioClip[] clips, Vector3 pos, float volume = 1f) =>
            AudioManager.PlaySound(clips.Random(), pos, volume);

        public static void Play(
            this AudioClip clip,
            Vector3 pos,
            float volume = 1f,
            float pitch = 1f
        ) => AudioManager.PlaySound(clip, pos, volume, pitch);

        public static void Play(
            this AudioClip[] clips,
            Vector3 pos,
            float volume = 1f,
            float pitch = 1f
        ) => AudioManager.PlaySound(clips.Random(), pos, volume, pitch);
    }
}
