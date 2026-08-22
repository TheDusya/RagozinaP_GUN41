using Assets.Scripts.Utilities;
using System;
using Zenject;

namespace Assets.Scripts.NPCs
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
 