using Assets.Scripts.NPCs;
using Assets.Scripts.Utilities;
using System;

namespace Assets.Scripts.NPCs
{
    public class AttackState : IState, IDisposable
    {
        DestructableGameObject _target;
        IAttacker _attacker;
        NPCSignalBus _signalBus;
        public void SetTarget(DestructableGameObject target) => _target = target;
        public AttackState(NPCSignalBus signalBus, IAttacker attacker, DestructableGameObject target = null)
        {
            _signalBus = signalBus;
            _attacker = attacker;
            _target = target;
        }
        public void Enter() => _signalBus.AttackMoment += OnAttack;
        public void OnAttack() => _target.TakeDamage(_attacker.GetAttackNum());
        public void Tick() {}
        public void Exit() => _signalBus.AttackMoment -= OnAttack;
        public void Dispose() =>_signalBus.AttackMoment -= OnAttack;
    }
}
