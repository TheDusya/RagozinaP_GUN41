using Assets.Scripts.Enemies.NPCStateMachines;
using Assets.Scripts.Parameters;
using Assets.Scripts.Utilities;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Enemies
{
    public class FighterStateMachine : StateMachine, ITickable, IDisposable
    {
        NPCSignalBus _signalBus;

        PatrolState _patrolState;
        ChaseState _chaseState;
        AttackState _attackState;
        GetHitState _getHitState;
        BackstepState _backstepState;
        DeadState _deadState;

        IState _currentState;

        public FighterStateMachine(Path path, Transform transform, NPCSignalBus signalBus, NPCParameters parameters)
        {
            _signalBus = signalBus;
            _patrolState = new PatrolState(path, transform, parameters.Speed);
            _currentState = _patrolState;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
