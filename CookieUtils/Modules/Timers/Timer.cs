using System;
using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.Timers
{
    [PublicAPI]
    public abstract class Timer : IDisposable
    {
        /// <summary>
        /// Construct the timer with an initial time and a host
        /// </summary>
        /// <param name="initialTime">The time to start with</param>
        /// <param name="host">The host, automatically disposed when the host is destroyed - set to null for no disposal</param>
        protected Timer(float initialTime, MonoBehaviour host)
        {
            InitialTime = initialTime;
            Host = host;
            CurrentTime = InitialTime;
            if (host == null) ShouldCancel = false;
        }

        protected readonly MonoBehaviour Host;
        protected readonly bool ShouldCancel = true;
        public float CurrentTime { get; protected set; }
        public float InitialTime { get; protected set; }
        public bool IsRunning { get; protected set; }

        public virtual string GetDisplayTime(string formatOverride = null)
        {
            return TimeSpan.FromSeconds(CurrentTime).ToString(formatOverride ?? "mm':'ss'.'fff");
        }
        
        /// <summary>
        /// Called by the TimerManager every update, used for ticking the timer
        /// </summary>
        public abstract void Tick();
        public abstract void Start();
        public abstract void Restart();
        public abstract void Restart(float newTime);
        public abstract void Stop();
        public abstract void Pause();
        public abstract void Resume();

        ~Timer()
        {
            ReleaseUnmanagedResources();
        }

        private void ReleaseUnmanagedResources()
        {
            TimerManager.DeregisterTimer(this);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
    }
    
    [PublicAPI]
    public class CountdownTimer : Timer
    {
        public Action OnComplete = null;
        public bool Loop = false;
        public bool IgnoreTimeScale = false;

        /// <inheritdoc />
        public CountdownTimer(float initialTime, MonoBehaviour host) : base(initialTime, host)
        {
        }

        /// <inheritdoc />
        public override void Tick()
        {
            if (ShouldCancel && !Host) {
                Dispose();
                return;
            }
            
            if (!IsRunning) {
                return;
            }

            CurrentTime -= IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            if (CurrentTime <= 0) {
                OnComplete?.Invoke();
                
                if (!Loop) {
                    Stop();
                    return;
                }

                Restart();
            }
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public override void Start()
        {
            if (IsRunning) return;
            
            IsRunning = true;
            CurrentTime = InitialTime;
            TimerManager.RegisterTimer(this);
        }

        /// <summary>
        /// Restarts the timer to the initial time, will not work if it hasn't been started
        /// </summary>
        public override void Restart()
        {
            CurrentTime = InitialTime;
            IsRunning = true;
        }

        /// <summary>
        /// Restarts the timer to the specified time, will not work if it hasn't been started
        /// </summary>
        /// <param name="newTime">The new time to start the timer with</param>
        public override void Restart(float newTime)
        {
            InitialTime = newTime;
            Restart();
        }

        /// <summary>
        /// Stops the timer. Pause() should be used if it's changed often
        /// </summary>
        public override void Stop()
        {
            IsRunning = false;
            TimerManager.DeregisterTimer(this);
        }

        /// <summary>
        /// Pauses the timer
        /// </summary>
        public override void Pause()
        {
            IsRunning = false;
        }

        /// <summary>
        /// Resumes the paused timer, does nothing if it's already running
        /// </summary>
        public override void Resume()
        {
            IsRunning = true;
        }
    }
}