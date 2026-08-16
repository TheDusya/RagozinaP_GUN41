using Assets.Scripts.Interfaces;
using Assets.Scripts.Parameters;
using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class PatrolState : IState
    {
        Vector3[] _points;
        Vector3 _previousPosition;
        Transform _transform;
        Tween _movementTween = null;
        public PatrolState(Path path, Transform transform, NPCParameters parameters)
        {
            _transform = transform;
            _points = path.Points;
           // _points = _points.Select(point => new Vector3(point.x + path.transform.position.x, point.y, point.z + path.transform.position.z)).ToArray();

            _movementTween = transform.DOPath(_points, parameters.Speed).
                SetLookAt(0.01f).
                SetSpeedBased(true).
                SetEase(Ease.Linear).
                SetLoops(-1, LoopType.Yoyo).
                OnUpdate(OnPathUpdate);
        }

        private void OnPathUpdate()
        {
            Vector3 currentPosition = _transform.position;
            Vector3 direction = currentPosition - _previousPosition;

            if (direction.magnitude > 0.001f)
            {
                _transform.rotation = Quaternion.LookRotation(direction.normalized);
            }

            _previousPosition = currentPosition;
        }

        public void Enter()
        {
            _movementTween.Play();
        }

        public void Tick()
        {
        }

        public void Exit()
        {
            _movementTween.Pause();
        }
    }
}
