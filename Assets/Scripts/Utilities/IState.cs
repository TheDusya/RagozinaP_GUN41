namespace Assets.Scripts.Utilities
{
    public interface IState
    {
        public void Enter();
        public void Tick();
        public void Exit();
    }
}
