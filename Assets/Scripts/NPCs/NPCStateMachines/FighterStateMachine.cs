using Assets.Scripts.NPCs.NPCStateMachines;
using Assets.Scripts.Parameters;
using Assets.Scripts.Utilities;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.NPCs
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

        public FighterStateMachine(Path path, Enemy me, NPCSignalBus signalBus, NPCParameters parameters)
        {
            _signalBus = signalBus;
            _patrolState = new PatrolState(path, me.transform, me.Speed);
            _chaseState = new ChaseState(me.transform, me.Speed);
            _attackState = new AttackState(signalBus, me);
            _getHitState = new GetHitState();
            _backstepState = new BackstepState(signalBus, path, me.transform, me.Speed);
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
