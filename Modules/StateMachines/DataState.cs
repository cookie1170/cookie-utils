using System;

namespace CookieUtils.StateMachines
{
    [Serializable]
    public abstract class State<THost, TData> : State<THost>
    {
        protected abstract TData DefaultData { get; }
        protected TData LastData { get; private set; }

        public void Enter(TData data, bool keepObjectActive)
        {
            if (gameObject)
                gameObject.SetActive(true);

            StateMachine.ChangeState(this, keepObjectActive);

            LastData = data;
            OnEnter(data);
        }

        public void Enter(TData data) => Enter(data, false);

        public new void Enter() => Enter(DefaultData);

        public new void Enter(bool keepObjectActive) => Enter(DefaultData, keepObjectActive);

        protected sealed override void OnEnter() { }

        protected virtual void OnEnter(TData data) { }
    }
}
