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
        public PatrolState(Path path, Transform transform, float speed)
        {
            _transform = transform;
            _points = path.Points;
            _movementTween = transform.DOPath(_points, speed).
                SetLookAt(0.01f).
                SetSpeedBased(true).
                SetEase(Ease.Linear).
                SetLoops(-1, LoopType.Yoyo).
                OnUpdate(OnPathUpdate);
            _movementTween.SetLink(transform.gameObject);
        }

        private void OnPathUpdate()
        {
            Vector3 currentPosition = _transform.position;
            Vector3 direction = currentPosition - _previousPosition;

            if (direction.magnitude > 0.001f)
                _transform.rotation = Quaternion.LookRotation(direction.normalized);

            _previousPosition = currentPosition;
        }

        public void Enter() => _movementTween.Play();
        public void Tick() {}
        public void Exit() => _movementTween.Pause();
    }
}
