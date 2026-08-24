using Assets.Scripts.NPCs.NPCStates;
using Assets.Scripts.Parameters;
using System;
using UnityEngine;

namespace Assets.Scripts.NPCs.NPCStateMachines
{
    public class FighterStateMachine : StateMachine, IDisposable
    {
        NPCSignalBus _signalBus;

        PatrolState _patrolState;
        ChaseState _chaseState;
        AttackState _attackState;
        GetHitState _getHitState;
        BackstepState _backstepState;
        DeadState _deadState;

        public FighterStateMachine(Path path, Enemy thisCharacter, NPCSignalBus signalBus, NPCParameters parameters) : base()
        {
            _signalBus = signalBus;
            _patrolState = new PatrolState(path, thisCharacter);
            _chaseState = new ChaseState(thisCharacter);
            _attackState = new AttackState(signalBus, thisCharacter);
            _getHitState = new GetHitState();
            _backstepState = new BackstepState(signalBus, path, thisCharacter);
            _deadState = new DeadState();
            _currentState = _patrolState;
            _currentState.Enter();

            _signalBus.TargetDetected += ChaseTarget;
            _signalBus.Attack += Attack;
            _signalBus.GetHit += GetHit;
            _signalBus.TargetLost += Backstep;
            _signalBus.Die += Die;
            _signalBus.BackOnTrack += Patrol;
        }

        private void ChaseTarget(Transform target)
        {
            _chaseState.SetTarget(target);
            SwitchTo(_chaseState);
        }

        private void Attack() => SwitchTo(_attackState);
        private void GetHit(float _) => SwitchTo(_getHitState);
        private void Backstep() => SwitchTo(_backstepState);
        private void Die() => SwitchTo(_deadState);
        private void Patrol() => SwitchTo(_patrolState);

        public void Dispose()
        {
            _signalBus.TargetDetected -= ChaseTarget;
            _signalBus.Attack -= Attack;
            _signalBus.GetHit -= GetHit;
            _signalBus.TargetLost -= Backstep;
            _signalBus.Die -= Die;
            _signalBus.BackOnTrack -= Patrol;
        }
    }
}
