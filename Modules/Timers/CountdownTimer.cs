using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CookieUtils.Timers
{
    [PublicAPI]
    public class CountdownTimer : Timer
    {
        public bool IgnoreTimeScale = false;
        public bool Loop = false;
        public Action OnComplete = null;

        /// <inheritdoc />
        public CountdownTimer(float initialTime)
            : base(initialTime) { }

        /// <inheritdoc />
        public override void Update()
        {
            if (!IsRunning)
                return;

            CurrentTime -= IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            if (CurrentTime <= 0)
            {
                OnComplete?.Invoke();

                if (!Loop)
                {
                    Stop();

                    return;
                }

                Restart();
            }
        }

        /// <summary>
        ///     Starts the timer
        /// </summary>
        public override void Start()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            CurrentTime = InitialTime;
        }

        /// <summary>
        ///     Restarts the timer to the initial time
        /// </summary>
        public override void Restart()
        {
            CurrentTime = InitialTime;
            IsRunning = true;
        }

        /// <summary>
        ///     Restarts the timer to the specified time
        /// </summary>
        /// <param name="newTime">The new time to start the timer with</param>
        public override void Restart(float newTime)
        {
            InitialTime = newTime;
            Restart();
        }

        /// <summary>
        ///     Stops the timer and resets its time
        /// </summary>
        public override void Stop()
        {
            IsRunning = false;
            CurrentTime = InitialTime;
        }

        /// <summary>
        ///     Pauses the timer
        /// </summary>
        public override void Pause()
        {
            IsRunning = false;
        }

        /// <summary>
        ///     Resumes the paused timer, does nothing if it's already running
        /// </summary>
        public override void Resume()
        {
            IsRunning = true;
        }

        public CountdownTimer AddTo(Object host)
        {
            Updater.AddTo(this, host);

            return this;
        }
    }
}
