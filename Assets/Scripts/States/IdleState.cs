using Assets.Scripts;
using Assets.Scripts.States;
using UnityEngine;
using Zenject;

public class IdleState : IState
{
    float _idleTime = 0;
    [Inject]
    Animator _animator;

    void IState.Enter() => IdleTimeReset();
    void IState.Update() => AddIdleTime(Time.deltaTime);
    void IState.Exit() => IdleTimeReset(); //2 times just in case

    public void AddIdleTime(float time)
    {
        _idleTime += time;
        if (_idleTime > Constants.idleTimeLimit)
            IdleTimeOver();
    }
    public void IdleTimeOver() => _animator.SetBool(Constants.idleTimeOverParName, true);
    public void IdleTimeReset()
    {
        _idleTime = 0;
        _animator.SetBool(Constants.idleTimeOverParName, false);
    }
}
