using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [SerializeField]
        private float _FOVRadius;
        public float FOVRadius { get => _FOVRadius; }
        [SerializeField, Range(0, 360)]
        private float _FOVAngle;
        public float FOVAngle { get => _FOVAngle; }

    }
}
