using Assets.Scripts.States;
using UnityEngine;
using Zenject;

public class CollectState : IState
{
    private const string treasureTouchedParName = "treasureTouched";
    [Inject]
    Animator _animator;

    void IState.Enter() { }
    void IState.Update() {}
    void IState.Exit() => TreasureReset();
    public void TreasureReset() => _animator.SetBool(treasureTouchedParName, false);
}
