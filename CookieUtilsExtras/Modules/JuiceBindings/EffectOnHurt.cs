namespace CookieUtils.Extras.Juice.Bindings
{
    public class EffectOnHurt : Effect
    {
        protected Health.Health Health;

        protected override void Awake()
        {
            base.Awake();
            Health = GetComponent<Health.Health>();
        }

        protected virtual void OnEnable()
        {
            Health.onHit.AddListener(OnTrigger);
        }

        protected void OnTrigger(Health.Health.HitInfo info)
        {
            Play(info.Direction);
        }

        protected virtual void OnDisable()
        {
            Health.onHit.RemoveListener(OnTrigger);
        }
    }
}
