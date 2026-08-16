using Assets.Scripts.NPCs;
using Assets.Scripts.Parameters;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.NPCs
{
    public class EvilDoctor : Enemy, IAttacker
    {
        [Inject]
        EvilDoctorParameters _parameters;
        float _strikePower;

        private void Start()
        {
            if (_path == null || !_path.Points.Any())
                Debug.LogError("Not a valid path");
            _stateMachine = new FighterStateMachine(_path, transform, _signalBus, _parameters);
            transform.position = _path.Points[0];
        }
        public float GetAttackNum() => _strikePower;

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
            throw new System.NotImplementedException();
        }
    }
}
