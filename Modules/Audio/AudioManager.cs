using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace CookieUtils.Audio
{
    public partial class AudioManager : Singleton<AudioManager>
    {
        private ObjectPool<AudioSource> _audioPool;

        protected override void Awake()
        {
            base.Awake();

            _audioPool = new ObjectPool<AudioSource>(
                () => Instantiate(AudioSettings.Get().sourcePrefab, transform),
                source => source.gameObject.SetActive(true),
                source => source.gameObject.SetActive(false),
                source => Destroy(source.gameObject),
                false,
                10,
                15
            );
        }

        public static void PlaySound(AudioClip clip, Vector3 position, AudioConfig config)
        {
            Instance.PlaySoundInstance(clip, position, config);
        }

        private async void PlaySoundInstance(AudioClip clip, Vector3 position, AudioConfig config)
        {
            AudioSource source = _audioPool.Get();
            source.clip = clip;
            source.transform.position = position;
            SetAudioSourceParameters(source, config);

            if (config.delay > Mathf.Epsilon)
                source.PlayDelayed(config.delay);
            else
                source.Play();

            float length = clip.length;

            await Awaitable.WaitForSecondsAsync(Mathf.Abs(length / source.pitch));

            if (source)
                _audioPool.Release(source);
        }

        private static void SetAudioSourceParameters(AudioSource source, AudioConfig config)
        {
            float volumeMult = Random.Range(config.minVolumeMult, config.maxVolumeMult);
            float pitchMult = Random.Range(config.minPitchMult, config.maxPitchMult);
            source.volume = config.volume * volumeMult;
            source.pitch = config.pitch * pitchMult;
            source.spatialBlend = config.spatialBlend;
            source.minDistance = config.minDistance;
            source.maxDistance = config.maxDistance;
            source.bypassEffects = config.bypassEffects;
            source.bypassReverbZones = config.bypassReverbZones;
            source.outputAudioMixerGroup = config.outputGroup;
        }

        [Serializable]
        public struct AudioConfig
        {
            [Range(0, 3)]
            public float delay;

            [Range(0, 1)]
            public float volume;

            [Range(0, 1)]
            public float minVolumeMult;

            [Range(0, 1)]
            public float maxVolumeMult;

            [Range(-3f, 3f)]
            public float pitch;

            [Range(-3f, 3f)]
            public float minPitchMult;

            [Range(-3f, 3f)]
            public float maxPitchMult;

            [Range(0, 1)]
            public float spatialBlend;

            public float minDistance;
            public float maxDistance;

            public bool bypassEffects;
            public bool bypassReverbZones;

            public AudioMixerGroup outputGroup;
        }
    }
}
