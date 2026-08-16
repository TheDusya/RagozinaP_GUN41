using UnityEngine;

namespace Assets.Scripts.Parameters
{
    public abstract class NPCParameters : ScriptableObject
    {
        [SerializeField]
        private int _maxHealth;
        public int MaxHealth { get => _maxHealth; }

        [SerializeField]
        private float _speed;
        public float Speed { get => _speed; }

        [SerializeField]
        private float _strikePower;
        public float StrikePower { get => _strikePower; }

    }
}
