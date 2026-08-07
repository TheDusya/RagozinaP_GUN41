using System;
using Zenject;

namespace Assets.Scripts
{
    internal class DefaultDestructableScenery : DestructableGameObject
    {
        [Inject]
        SceneryParameters _sceneryParameters;
        public override void SetParameters()
        {
            _maxHealth = _sceneryParameters.DefaultSceneryHealth;
            _health = _maxHealth;
        }
        public override void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
                Die();
            //Will VFX be here too?
        }
        public override void Die()
        {
            throw new NotImplementedException();
        }
    }
}
