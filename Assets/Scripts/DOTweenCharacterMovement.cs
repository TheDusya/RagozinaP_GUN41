using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    internal class DOTweenCharacterMovement : MonoBehaviour
    {
        Vector3[] points = null;
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
        private void DoMovement()
        {
            transform.DOPath(points, 10).SetLookAt(transform.position).SetLookAt(0.01f);
        }
    }
}
