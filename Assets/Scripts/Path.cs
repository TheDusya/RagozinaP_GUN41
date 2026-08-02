using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class Path : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => Pick();
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => UnPick();
    }
}
