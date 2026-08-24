using Assets.Scripts.NPCs.NPCStateMachines;
using Assets.Scripts.Parameters;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.NPCs
{
    public class EvilDoctor : Enemy
    {
        [Inject]
        EvilDoctorParameters _parameters;

        private void Start()
        {
            if (_path == null || !_path.Points.Any())
                Debug.LogError("Not a valid path");
            Speed = _parameters.Speed;

            _animationController = new DefaultEnemyAnimationController(_animator, _signalBus);
            _stateMachine = new FighterStateMachine(_path, this, _signalBus, _parameters);
            transform.position = _path.Points[0];
            if (!_path.IsSingle)
                _signalBus.OnWalk();
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        public override void SetParameters()
        {
            _maxHealth = _parameters.MaxHealth;
            _health = _parameters.MaxHealth;
            _strikePower = _parameters.StrikePower;
        }

        public override void TakeDamage(float damage)
        {
            _signalBus.OnGetHit(damage);
            _health = Mathf.Max(_health - damage, 0);
            if (_health == 0)
                Die();
        }

        public override void Die()
        {
        }
    }
}
