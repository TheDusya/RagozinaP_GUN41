using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{

    [Inject]
    SharedDataManager _dataManager;
    [SerializeField]
    private float _speed = 0.01f;
    [SerializeField]
    private Vector3 cellOffset;

    private Vector3 _movementStart;
    private Vector3 _movementEnd;
    private Transform _unitTransform;
    float _time;

    private void Awake()
    {
        _dataManager.OnGameEvent += ProcessEvent;
    }

    private void ProcessEvent(GameEvent gameEvent)
    {
        switch (gameEvent)
        {
            case GameEvent.Confirm:
                _movementStart = _dataManager.Unit.gameObject.transform.position;
                _movementEnd = _dataManager.Cell.transform.position + cellOffset;
                _unitTransform = _dataManager.Unit.gameObject.transform;
                _time = 0;
                break;
            case GameEvent.Attack:
                Destroy(_dataManager.AttackedUnit);
                break;
        }
    }

    private void Update()
    {
        if (_dataManager.CurrentState != State.Lock)
            return;
        _time += Time.deltaTime * _speed;

        if (_time >= 1f)
        {
            _unitTransform.position = _movementEnd;
            _dataManager.MovementEnd();
        }
        else
            _unitTransform.position = Vector3.Lerp(_movementStart, _movementEnd, _time);

    }
    private void OnDestroy()
    {
        _dataManager.OnGameEvent += ProcessEvent;
    }
}
