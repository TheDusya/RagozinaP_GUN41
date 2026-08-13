namespace Assets.Scripts.Interfaces
{
    public interface IState
    {
        public void Enter();
        public void LateTick();
        public void Exit();
    }
}
