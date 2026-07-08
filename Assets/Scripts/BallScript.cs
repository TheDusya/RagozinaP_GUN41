using UnityEngine;
using Zenject;

public class BallScript : MonoBehaviour
{
    [Inject]
    private (float min, float max) _xRange;
    [Inject(Id = "Start")]
    private GameObject _startingPoint;

    [Inject]
    void AfterInject() => SetRandomStart();

    private void SetRandomStart()
    {
        float startingX = (float)(_xRange.min + new System.Random().NextDouble() * (_xRange.max - _xRange.min));
        var myPosition = transform.position;
        var pointPosition = _startingPoint.transform.position;
        transform.position = new Vector3(startingX, myPosition.y, myPosition.z);
        _startingPoint.transform.position = new Vector3 (startingX, pointPosition.y, pointPosition.z);
    }
}
