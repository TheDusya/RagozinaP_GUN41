using Assets.Scripts.Utilities;

namespace Assets.Scripts.NPCs.NPCStateMachines
{
    public abstract class StateMachine
    {
        protected IState _currentState;
        public void SwitchTo(IState newState)
        {
            _currentState.Exit();
            _currentState = newState;
            newState.Enter();
        }
        public void Tick() => _currentState.Tick();
    }
}
 