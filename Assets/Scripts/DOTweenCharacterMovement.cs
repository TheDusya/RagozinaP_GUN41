using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    internal class DOTweenCharacterMovement : MonoBehaviour
    {
        Vector3[] points = null;
        bool _isMoving = false;
        Tween _movementTween = null;
        public bool IsMoving { get => _isMoving; }
        void Start()
        {
            var renderer = GetComponentInChildren<Renderer>();
            renderer.materials.FirstOrDefault().
                DOColor(Color.blue, GameParameters.ColorChangingTime). //Персонаж будет красиво синеть
                SetLoops(-1, LoopType.Yoyo). //бесконечный луп цвета туда-сюда
                SetId(GameParameters.ColorDOTweenTag); //тег для поиска на всякий случай
            InGameEventManager.PathWasChosen += SetPath;
        }
        void OnDestroy() => InGameEventManager.PathWasChosen -= SetPath;

        private void SetPath(Scripts.Path path)
        {
            if (path == null)
                Debug.Log("Not a valid movement path!");
            else if (path.Points == null || !path.Points.Any())
                Debug.Log("No points for movement found in path!");
            else
            {
                points = path.Points;
                DoMovement();
            }
        }

        private void Update()
        {
            if (_isMoving && !_movementTween.IsActive())
                StopMovement();
        }

        private void DoMovement()
        {
            _isMoving = true;
            _movementTween = transform.DOPath(points, GameParameters.BasicMovementSpeed). //путь по точкам
                SetLookAt(transform.position). //персонаж "смотрит", куда идет
                SetLookAt(0.01f). //процент "осматриваемого" пути
                SetSpeedBased(true). //движение задаётся через скорость
                SetEase(Ease.Linear). //без плавного затухания движения
                SetId(GameParameters.MovementDOTweenTag); //тег для поиска на всякий случай
        }

        private void StopMovement()
        {
            _isMoving = false;
            InGameEventManager.FinishPath();
        }
    }
}
