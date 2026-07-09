using Assets.Scripts;
using System.Collections.Generic;
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
    [Inject]
    private GameParameters _gameParameters;
    private bool _isPointChosen;
    private Rigidbody _rigidbody;

    [Inject]
    void AfterInject()
    {
        if (!TryGetComponent<Rigidbody>(out _rigidbody))
            throw new System.Exception("Rigidbody not found");
        if (!TryGetComponent<Renderer>(out var renderer))
            throw new System.Exception("Renderer not found (for some reason)");
        renderer.SetMaterials(new List<Material>() { _gameParameters.GetMaterialByType(_gameParameters.BallType), renderer.materials[1] });
        _rigidbody.mass = _gameParameters.GetMassByType(_gameParameters.BallType);
        _rigidbody.useGravity = false;
        SetRandomStart();
    }

    private Vector3 VectorWithX(Vector3 position, float x) => new Vector3(x, position.y, position.z);
    private void SetXForBallAndStartingPoint(float x)
    {
        transform.position = VectorWithX(transform.position, x);
        _startingPoint.transform.position = VectorWithX(_startingPoint.transform.position, x);
    }

    void OnDrawGizmos() //for me to see the range
    {
        if (_isPointChosen)
            return;
        Handles.color = Color.blue;
        Handles.DrawLine(VectorWithX(transform.position, _xRange.min), VectorWithX(transform.position, _xRange.max));
    }

    private void SetRandomStart()
    {
        float startingX = (float)(_xRange.min + new System.Random().NextDouble() * (_xRange.max - _xRange.min));
        SetXForBallAndStartingPoint(startingX);
        _isPointChosen = false;
    }

    void FixedUpdate()
    {
        if (!_isPointChosen)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100f) &&
                hit.point.x >= _xRange.min && hit.point.x <= _xRange.max)
                SetXForBallAndStartingPoint(hit.point.x);
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        _isPointChosen = true;
        _rigidbody.useGravity = true;
        _startingPoint.SetActive(false);
        _rigidbody.AddForce(new Vector3(0, 0, 1) * _gameParameters.PunchPower, ForceMode.Impulse);
    }
}
