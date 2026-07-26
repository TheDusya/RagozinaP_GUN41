namespace Assets.Scripts.States
{
    public interface IState
    {
        abstract void Enter();
        abstract void Update();
        abstract void Exit();

    }
}
