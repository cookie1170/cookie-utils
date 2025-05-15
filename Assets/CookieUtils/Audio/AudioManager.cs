using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace CookieUtils.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Inst { get; private set; }
        
        [SerializeField] private AudioSource audioPrefab;
        [SerializeField] private Transform audioContainer;

        private ObjectPool<AudioSource> _audioPool;

        private void Awake()
        {
            if (Inst) Destroy(Inst.gameObject);
            Inst = this;
            _audioPool = new(() => Instantiate(audioPrefab, parent: audioContainer),
            source => source.gameObject.SetActive(true),
            source => source.gameObject.SetActive(false),
            source => Destroy(source.gameObject),
            false, 10, 15);
        }

        public void PlaySfx(AudioClip clip, Transform position = null, float volume = 1f)
        {
            AudioSource source = _audioPool.Get();
            source.clip = clip;
            source.volume = volume;
            source.transform.position = position == null ? Camera.current.transform.position : position.position;
            source.Play();
            float length = clip.length;
            DOVirtual.DelayedCall(length, () => _audioPool.Release(source));
        }
    }
}
