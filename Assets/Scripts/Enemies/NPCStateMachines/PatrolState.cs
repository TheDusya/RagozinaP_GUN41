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
        Transform _transform;
        Tween _movementTween = null;
        public PatrolState(Path path, Transform transform, NPCParameters parameters)
        {
            _points = path.Points;
            _points = _points.Select(point => new Vector3(point.x + path.transform.position.x, point.y, point.z + path.transform.position.z)).ToArray();

            _movementTween = transform.DOPath(_points, parameters.Speed). //путь по точкам
                SetLookAt(0.01f). //процент "осматриваемого" пути
                SetSpeedBased(true). //движение задаётся через скорость
                SetEase(Ease.OutBack). //без плавного затухания движения
                SetLoops(-1);

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
