namespace Assets.Scripts.Interfaces
{
    public interface IState
    {
        public void Enter();
        public void Tick();
        public void Exit();
    }
}
