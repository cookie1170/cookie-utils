using UnityEngine;

namespace CookieUtils.ObjectPooling
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ReleaseParticleOnFinish : MonoBehaviour, IPoolCallbackReceiver
    {
        private ParticleSystem[] _particles;
        private bool _playOnAwake;

        private void Awake()
        {
            _particles = GetComponentsInChildren<ParticleSystem>();

            ParticleSystem.MainModule firstMain = _particles[0].main;
            firstMain.stopAction = ParticleSystemStopAction.Callback;
            _playOnAwake = firstMain.playOnAwake;

            foreach (ParticleSystem particles in _particles)
            {
                ParticleSystem.MainModule main = particles.main;
                main.loop = false;
                main.playOnAwake = false;
            }
        }

        private void OnParticleSystemStopped()
        {
            this.Release();
        }

        public void OnGet()
        {
            if (_playOnAwake)
                _particles[0].Play();
        }

        public void OnRelease() { }
    }
}
