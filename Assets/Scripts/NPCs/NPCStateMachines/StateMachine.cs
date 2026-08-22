using Assets.Scripts.Utilities;
using System;

namespace Assets.Scripts.NPCs
{
    public abstract class StateMachine
    {
        private IState _currentState;
        public void SwitchTo(IState newState)
        {
            try {
                _currentState.Exit();
                _currentState = newState;
                newState.Enter();
            }
            catch(Exception e)
            {

            }
        }
        public void Tick() => _currentState.Tick();
    }
}
 