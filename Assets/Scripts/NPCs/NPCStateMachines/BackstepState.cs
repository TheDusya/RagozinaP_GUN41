using Assets.Scripts.Utilities;
using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.NPCs.NPCStateMachines
{
    internal class BackstepState : IState
    {
        NPCSignalBus _signalBus;
        Path _homePath;
        Transform _transform;
        Tween _movementTween = null;
        float _speed;

        public BackstepState(NPCSignalBus signalBus, Path path, Transform transform, float speed)
        {
            _homePath = path;
            _transform = transform;
            _speed = speed;
            _signalBus = signalBus;
        }
        public void Enter()
        {
            if (!_homePath.Points.Any())
                Debug.LogError("No points!");
            var closestPoint = _homePath.Points.OrderBy(p => (p - _transform.position).sqrMagnitude).First();
            _movementTween = _transform.DOMove(closestPoint, _speed).
                SetSpeedBased(true).
                SetEase(Ease.Linear);
            _movementTween.SetLink(_transform.gameObject);
            _movementTween.onComplete += _signalBus.OnBackOnTrack;
        }

        public void Tick() {}

        public void Exit()
        {
            _movementTween.onComplete -= _signalBus.OnBackOnTrack;
            _movementTween.Kill();
        }
    }
}
