using Assets.Scripts.Enemies.EnemyStateMachines;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Parameters;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Enemies
{
    public class FighterStateMachine : StateMachine, ITickable
    {
        NPCSignalBus _signalBus;

        PatrolState _patrolState;
        ChaseState _chaseState;
        AttackState _attackState;
        BackstepState _backstepState;
        DeadState _deadState;

        IState _currentState;

        public FighterStateMachine(Path path, Transform transform, NPCSignalBus signalBus, NPCParameters parameters)
        {
            _signalBus = signalBus;
            _patrolState = new PatrolState(path, transform, parameters);
            _currentState = _patrolState;
        }
    }
}
