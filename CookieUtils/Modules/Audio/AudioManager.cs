using UnityEngine;
using UnityEngine.Pool;

namespace CookieUtils.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource audioPrefab;

        private ObjectPool<AudioSource> _audioPool;

        protected override void Awake()
        {
            base.Awake();
            
            _audioPool = new(() => Instantiate(audioPrefab, parent: transform),
            source => source.gameObject.SetActive(true),
            source => source.gameObject.SetActive(false),
            source => Destroy(source.gameObject),
            false, 10, 15);
        }

        public static void PlaySound(AudioClip clip, float volume, Vector3 position, bool directional)
        {
            Inst.PlaySoundInst(clip, volume, position, directional);
        }
        
        private async void PlaySoundInst(AudioClip clip, float volume, Vector3 position, bool directional)
        {
            AudioSource source = _audioPool.Get();
            source.clip = clip;
            source.volume = volume;
            source.transform.position = position;
            source.pitch = Random.Range(0.8f, 1.2f);
            source.dopplerLevel = directional ? 1 : 0;
            source.Play();
            float length = clip.length;
            await Awaitable.WaitForSecondsAsync(length + 0.2f);
            if (source) _audioPool.Release(source);
        }
    }
}
