using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Enemies
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
            _target = target;
            _speed = speed;
        }
        public void Enter()
        {
            if (_target == null)
                Debug.LogError("Target not found!");
        }

        public void Tick()
        {
            var diff = _myTransform.position - _target.position;
            var movementVector = diff.normalized;
            var moveVec = movementVector * Time.deltaTime * _speed;
            if (moveVec.sqrMagnitude > diff.sqrMagnitude && diff.sqrMagnitude > 0.001)
                _myTransform.position += moveVec;
        }

        public void Exit() {}
    }
}
