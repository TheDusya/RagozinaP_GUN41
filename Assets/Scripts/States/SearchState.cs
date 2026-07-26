using Assets.Scripts;
using Assets.Scripts.States;
using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class SearchState : IState, IDisposable
{
    [Inject]
    SearchTrigger _searchTrigger;
    [Inject]
    NavMeshAgent _agent;
    [Inject]
    Animator _animator;

    Renderer _followedObjectRenderer;
    bool IsTreasureFound => _followedObjectRenderer != null;

    [Inject]
    public void OnInject() => _searchTrigger.OnTriggerEnterEvent += OnTrigger;
    void IState.Enter()
    {
        _followedObjectRenderer = null;
        SetRandomGoal();
    }
    void IState.Update()
    {
        if (_agent.pathPending == false && _agent.remainingDistance <= _agent.stoppingDistance)
            if (!IsTreasureFound)
                SetRandomGoal();
            else
                TreasureTouched();
    }
    void IState.Exit() => _followedObjectRenderer = null;

    public void OnTrigger(GameObject gameObject)
    {
        if (IsTreasureFound)
            return;
        if (!NavMesh.SamplePosition(gameObject.transform.position, out var hit, 1, NavMesh.AllAreas))
            return;
        if (!gameObject.TryGetComponent<Renderer>(out _followedObjectRenderer))
            throw new Exception("No renderer!");
        SetGoal(hit.position);
    }

    void IDisposable.Dispose()
    {
        if (_searchTrigger.OnTriggerEnterEvent != null)
            _searchTrigger.OnTriggerEnterEvent -= OnTrigger;
    }

    public void SetRandomGoal()
    {
        var savedY = _agent.transform.position.y;
        Vector3 randomPosition = Vector3.zero;
        for (int i = 0; i < Constants.maxRandomPointAttempts; i++)
        {
            var randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector.y = savedY;
            Vector3 randomPoint = _agent.transform.position + randomVector * Constants.maxStraightMoveRadius;
            if (NavMesh.SamplePosition(randomPoint, out var hit, 5, NavMesh.AllAreas))
            {
                randomPosition = hit.position;
                break;
            }
        }

        if (randomPosition != Vector3.zero)
            SetGoal(randomPosition);
        else
            return;
    }

    public void SetGoal(Vector3 position) => _agent.SetDestination(position);
    public void TreasureTouched() 
    { 
        _animator.SetBool(Constants.treasureTouchedParName, true);
        _followedObjectRenderer.enabled = false;
    }
}
