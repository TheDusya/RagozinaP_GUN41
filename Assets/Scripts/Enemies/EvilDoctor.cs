using Assets.Scripts.Parameters;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Enemies
{
    public class EvilDoctor : Enemy
    {
        [Inject]
        EvilDoctorParameters _parameters;
        [SerializeField]
        private float _FOVRadius;
        [SerializeField, Range(0, 360)]
        private float _FOVAngle;

        //[Inject]
        private void Start()
        {
            _stateMachine = new FighterStateMachine(_path, transform, _signalBus, _parameters);
        }
        public override void SetParameters()
        {
            throw new System.NotImplementedException();
        }

        public override void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }

        public override void Die()
        {
            throw new System.NotImplementedException();
        }
    }
}
