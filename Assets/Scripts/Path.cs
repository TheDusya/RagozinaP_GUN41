using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    internal class Path : MonoBehaviour
    {
        [Inject]
        GameParameters _gameParameters;

        LineRenderer _lineRenderer;
        public Vector3[] Positions { get; private set; }

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            var positionCount = _lineRenderer.positionCount;
            Positions = new Vector3[positionCount];
            _lineRenderer.GetPositions(Positions);
            _lineRenderer.material = _gameParameters.NotPickedPathMaterial;
        }

        public void Pick() => _lineRenderer.material = _gameParameters.PickedPathMaterial;
        public void UnPick() => _lineRenderer.material = _gameParameters.NotPickedPathMaterial;
    }
}
