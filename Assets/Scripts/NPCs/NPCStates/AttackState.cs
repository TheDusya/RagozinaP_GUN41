using Assets.Scripts.Utilities;
using System;
using UnityEngine;

namespace Assets.Scripts.NPCs.NPCStates
{
    public class AttackState : IState, IDisposable
    {
        DestructableGameObject _target;
        readonly IAttacker _attacker;
        readonly NPCSignalBus _signalBus;

        public void SetTarget(DestructableGameObject target) => _target = target;
        public AttackState(NPCSignalBus signalBus, IAttacker thisCharacter, DestructableGameObject target = null)
        {
            _signalBus = signalBus;
            _attacker = thisCharacter;
            if (target != null)
                _target = target;
        }
        public void Enter()
        {
            if (_target == null)
                Debug.LogError("Target not found!");
            _signalBus.AttackMoment += OnAttack;
        }
        public void OnAttack() => _target.TakeDamage(_attacker.GetAttackNum());
        public void Tick() {}
        public void Exit() => _signalBus.AttackMoment -= OnAttack;
        public void Dispose() =>_signalBus.AttackMoment -= OnAttack;
    }
}
