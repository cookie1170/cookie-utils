namespace CookieUtils.Extras.Juice.Bindings
{
    public class EffectOnHurt : Effect
    {
        protected HealthSystem.Health Health;

        protected override void Awake()
        {
            base.Awake();
            Health = GetComponent<HealthSystem.Health>();
        }

        protected virtual void OnEnable()
        {
            Health.onHit.AddListener(OnTrigger);
        }

        protected void OnTrigger(HealthSystem.Health.AttackInfo info)
        {
            Play(info.HitboxInfo.Direction, info.ContactPoint);
        }

        protected virtual void OnDisable()
        {
            Health.onHit.RemoveListener(OnTrigger);
        }
    }
}
