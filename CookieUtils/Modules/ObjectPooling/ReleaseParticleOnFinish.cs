using UnityEngine;

namespace CookieUtils.Runtime.ObjectPooling
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ReleaseParticleOnFinish : MonoBehaviour
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

        private void OnEnable()
        {
            if (_playOnAwake) _particles.Play();
        }

        private void OnParticleSystemStopped()
        {
            this.Release();
        }
    }
}