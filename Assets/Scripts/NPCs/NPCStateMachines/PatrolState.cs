using Assets.Scripts.Utilities;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    public class PatrolState : IState
    {
        Vector3[] _points;
        Vector3 _previousPosition;
        Transform _transform;
        Tween _movementTween = null;
        float _speed;
        public PatrolState(Path path, Transform transform, float speed)
        {
            _transform = transform;
            _points = path.Points;
            _speed = speed;
        }

        private void OnPathUpdate()
        {
            Vector3 currentPosition = _transform.position;
            Vector3 direction = currentPosition - _previousPosition;

            if (direction.magnitude > 0.001f)
                _transform.rotation = Quaternion.LookRotation(direction.normalized);

            _previousPosition = currentPosition;
        }

        public void Enter()
        {
            _movementTween = _transform.DOPath(_points, _speed). //pointslist? No idea
                SetLookAt(0.01f).
                SetSpeedBased(true).
                SetEase(Ease.Linear).
                SetLoops(-1, LoopType.Yoyo).
                OnUpdate(OnPathUpdate);
            _movementTween.SetLink(_transform.gameObject);
        }
        public void Tick() 
        { 
        }
        public void Exit() => _movementTween.Kill();
    }
}
