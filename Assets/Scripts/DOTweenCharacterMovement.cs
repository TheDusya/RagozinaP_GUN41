using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    internal class DOTweenCharacterMovement : MonoBehaviour
    {
        Vector3[] points = null;
        bool isMoving = false;
        void Start() => InGameEventManager.PathWasChosen += SetPath;
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
            if (isMoving && !DOTween.IsTweening(transform))
                StopMovement();
        }

        private void DoMovement()
        {
            isMoving = true;
            transform.DOPath(points, GameParameters.BasicMovementSpeed). //путь по точкам
                SetLookAt(transform.position). //персонаж "смотрит", куда идет
                SetLookAt(0.01f). //процент "осматриваемого" пути
                SetSpeedBased(true). //движение задаётся через скорость
                SetEase(Ease.Linear); //без плавного затухания движения
        }

        private void StopMovement()
        {
            isMoving = false;
            InGameEventManager.FinishPath();
        }
    }
}
