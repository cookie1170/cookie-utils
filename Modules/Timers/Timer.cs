using System;
using JetBrains.Annotations;

namespace CookieUtils.Timers
{
    [PublicAPI]
    public abstract class Timer : IUpdate, IDisposable
    {
        /// <summary>
        ///     Construct the timer with an initial time and a host
        /// </summary>
        /// <param name="initialTime">The time to start with</param>
        protected Timer(float initialTime)
        {
            Updater.Register(this);
            InitialTime = initialTime;
            CurrentTime = InitialTime;
        }

        private bool _isRunning;

        public float CurrentTime { get; protected set; }
        public float InitialTime { get; protected set; }
        public bool IsRunning
        {
            get => _isRunning;
            protected set
            {
                // we're fine to call it here without checks because the method checks itself
                if (value)
                    Updater.Register(this);

                _isRunning = value;
            }
        }

        public virtual string GetDisplayTime(string formatOverride = null) =>
            TimeSpan.FromSeconds(CurrentTime).ToString(formatOverride ?? "mm':'ss'.'fff");

        /// <summary>
        ///     Called by the Updater every update, used for ticking the timer
        /// </summary>
        public abstract void Update();

        public abstract void Start();
        public abstract void Restart();
        public abstract void Restart(float newTime);
        public abstract void Stop();
        public abstract void Pause();
        public abstract void Resume();

        public void Dispose()
        {
            Updater.Deregister(this);
        }
    }
}
