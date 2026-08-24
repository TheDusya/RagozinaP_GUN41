using Assets.Scripts.Utilities;
using System;
using UnityEngine;

namespace Assets.Scripts.NPCs.NPCStates
{
    public class ChaseState : IState
    {
        Transform _target;
        readonly Transform _transform;
        readonly float _speed;

        public void SetTarget(Transform target) => _target = target;
        public ChaseState(Character thisCharacter, Transform target = null)
        {
            _transform = thisCharacter.transform;
            _speed = thisCharacter.Speed;
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
            var diff = _target.position - _transform.position;
            if (diff.sqrMagnitude > 0.001)
                _transform.position = Vector3.MoveTowards(_transform.position, _target.position, Time.deltaTime * _speed);
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Exit() {}
    }
}
