using UnityEngine;

namespace CookieUtils.ObjectPooling
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ReleaseParticleOnFinish : MonoBehaviour, IPoolCallbackReceiver
    {
        private ParticleSystem _particles;
        private bool _playOnAwake;

        private void Awake()
        {
            _particles = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = _particles.main;
            main.stopAction = ParticleSystemStopAction.Callback;
            main.loop = false;
            _playOnAwake = main.playOnAwake;
            main.playOnAwake = false;
        }

        private void OnParticleSystemStopped()
        {
            this.Release();
        }

        public void OnGet()
        {
            if (_playOnAwake)
                _particles.Play();
        }

        public void OnRelease() { }
    }
}
