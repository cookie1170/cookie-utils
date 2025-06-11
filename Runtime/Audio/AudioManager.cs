using UnityEngine;
using UnityEngine.Pool;

namespace CookieUtils.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Inst { get; private set; }
        
        [SerializeField] private AudioSource audioPrefab;

        private ObjectPool<AudioSource> _audioPool;

        private void Awake()
        {
            if (Inst) Destroy(Inst.gameObject);
            Inst = this;
            _audioPool = new(() => Instantiate(audioPrefab, parent: transform),
            source => source.gameObject.SetActive(true),
            source => source.gameObject.SetActive(false),
            source => Destroy(source.gameObject),
            false, 10, 15);
        }

        public async void PlaySound(AudioClip clip, float volume, Vector3 position, bool directional)
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
            _audioPool.Release(source);
        }
    }
}
