using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class BallScript : MonoBehaviour, IPointerClickHandler
{
    [Inject]
    private (float min, float max) _xRange;
    [Inject(Id = "Start")]
    private GameObject _startingPoint;
    private bool _isPointChosen;

    [Inject]
    void AfterInject() => SetRandomStart();
    private Vector3 VectorWithX(Vector3 position, float x) => new Vector3(x, position.y, position.z);
    private void SetXForBallAndStartingPoint(float x)
    {
        transform.position = VectorWithX(transform.position, x);
        _startingPoint.transform.position = VectorWithX(_startingPoint.transform.position, x);
    }

    void OnDrawGizmos() //for me to see the range
    {
        Handles.color = Color.blue;
        Handles.DrawLine(VectorWithX(transform.position, _xRange.min), VectorWithX(transform.position, _xRange.max));
    }

    private void SetRandomStart()
    {
        float startingX = (float)(_xRange.min + new System.Random().NextDouble() * (_xRange.max - _xRange.min));
        SetXForBallAndStartingPoint(startingX);
        _isPointChosen = false;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!_isPointChosen && Physics.Raycast(ray, out var hit, 100f) && 
            hit.point.x >= _xRange.min && hit.point.x <= _xRange.max)
                SetXForBallAndStartingPoint(hit.point.x);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        _isPointChosen = true;
    }
}
