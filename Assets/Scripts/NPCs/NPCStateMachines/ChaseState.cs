using Assets.Scripts.Utilities;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.NPCs
{
    internal class ChaseState : IState
    {
        Transform _target;
        Transform _myTransform;
        float _speed;

        public void SetTarget(Transform target) => _target = target;

        public ChaseState(Transform myTransform, float speed, Transform target = null)
        {
            _myTransform = myTransform;
            _speed = speed;
            if (target != null)
                _target = target;
        }
        public void Enter()
        {
            if (_target == null)
                Debug.LogError("Target not found!");
        }

        public void Tick()
        {
            var diff = _target.position - _myTransform.position;
            if (diff.sqrMagnitude > 0.001)
                _myTransform.position = Vector3.MoveTowards(_myTransform.position, _target.position, Time.deltaTime * _speed);
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Exit() {}
    }
}
