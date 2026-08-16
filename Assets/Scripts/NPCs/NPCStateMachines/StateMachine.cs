using Assets.Scripts.Utilities;

namespace Assets.Scripts.Enemies
{
    public abstract class StateMachine
    {
        private IState _currentState;
        public void SwitchTo(IState newState)
        {
            _currentState.Exit();
            _currentState = newState;
            newState.Enter();
        }
        public void Tick() => _currentState.Tick();
    }
}
 