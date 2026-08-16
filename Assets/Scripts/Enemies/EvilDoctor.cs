using Assets.Scripts.Parameters;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Enemies
{
    public class EvilDoctor : Enemy
    {
        [Inject]
        EvilDoctorParameters _parameters;

        //[Inject]
        private void Start()
        {
            if (_path == null || !_path.Points.Any())
                Debug.LogError("Not a valid path");
            _stateMachine = new FighterStateMachine(_path, transform, _signalBus, _parameters);
            transform.position = _path.Points[0];
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
