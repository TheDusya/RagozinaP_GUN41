using Assets.Scripts.Utilities;
using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.NPCs.NPCStates
{
    public class BackstepState : IState, IDisposable
    {
        readonly NPCSignalBus _signalBus;
        readonly Path _homePath;
        readonly Transform _transform;
        Tween _movementTween = null;
        readonly float _speed;

        public BackstepState(NPCSignalBus signalBus, Path path, Character thisCharacter)
        {
            _homePath = path;
            _transform = thisCharacter.transform;
            _speed = thisCharacter.Speed;
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
        public void Dispose() => _movementTween.onComplete -= _signalBus.OnBackOnTrack;
    }
}
