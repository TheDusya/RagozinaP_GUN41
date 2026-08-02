using System.Linq;
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
            Points = Points.Select(point => new Vector3(point.x + transform.position.x, point.y, point.z + transform.position.z)).ToArray();

            UnPick();
            _isAnyPathChosen = false;
            InGameEventManager.PathWasChosen += MakeTheChoice;
            InGameEventManager.PathWasFinished += SetTheChoiceAvailableAgain;
        }

        private void OnDestroy()
        {
            InGameEventManager.PathWasChosen -= MakeTheChoice;
            InGameEventManager.PathWasFinished -= SetTheChoiceAvailableAgain;
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

        void SetTheChoiceAvailableAgain()
        {
            UnPick();
            _isAnyPathChosen = false;
        }

        void MakeTheChoice(Path path)
        {
            if (path.Equals(this))
                return;
            _isAnyPathChosen = true;
        }
    }
}
