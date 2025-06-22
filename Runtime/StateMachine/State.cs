namespace CookieUtils.StateMachine
{
    public abstract class State<T>
    {
        public T Host;
        public StateMachine<T> StateMachine;

        public virtual void Leave()
        {
            
        }

        public virtual void Enter()
        {
            
        }

        public virtual void Update()
        {
            
        }
        
        public virtual void FixedUpdate()
        {
            
        }
    }
}