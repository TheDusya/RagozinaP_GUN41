using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public abstract class DestructableGameObject : MonoBehaviour
    {
        protected float _maxHealth;
        protected float _health;
        public abstract void SetParameters();
        public abstract void TakeDamage(float damage);
        public abstract void Die();
    }
}
