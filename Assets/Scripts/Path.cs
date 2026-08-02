using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class Path : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Inject]
        GameParameters _gameParameters;
        [SerializeField]
        bool _isLooped;
        LineRenderer _lineRenderer;
        bool _isAnyPathChosen;
        public bool IsLooped { get => _isLooped; }
        public Vector3[] Points { get; private set; }

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            var positionCount = _lineRenderer.positionCount;
            Points = new Vector3[positionCount];
            _lineRenderer.GetPositions(Points);
            _lineRenderer.material = _gameParameters.NotPickedPathMaterial;
            _isAnyPathChosen = false;
        }

        public void Pick() => _lineRenderer.material = _gameParameters.PickedPathMaterial;
        public void UnPick() => _lineRenderer.material = _gameParameters.NotPickedPathMaterial;

        //наверное, стоило делать через подписку/отписку, но OnPointer<...> все равно удобнее
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (_isAnyPathChosen)
                return;
            Pick();
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (_isAnyPathChosen)
                return;
            UnPick();
        }
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (_isAnyPathChosen)
                return;
            Pick();
            _isAnyPathChosen = true;
            InGameEventManager.ChoosePath(this);
        }
    }
}
