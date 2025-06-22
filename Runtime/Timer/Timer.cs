namespace CookieUtils.Timer
{
    public abstract class Timer
    {

        public float Time { get; protected set; }

        public bool IgnoreTimeScale;
        public bool IsRunning;
        
        public abstract void Tick(float deltaTime);
        
        public void Release()
        {
            TimerManager.ReleaseTimer(this);
        }

        public virtual void Start()
        {
            IsRunning = true;
        }
    }
}