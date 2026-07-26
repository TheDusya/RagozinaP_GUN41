using Assets.Scripts.States;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StateBehaviour : StateMachineBehaviour
{
    IState _currentState = null;
    Dictionary<string, IState> _states = null;

    public void InjectAll() //oh, this is horrible
    {
        var ctx = UnityEngine.Object.FindObjectOfType<SceneContext>();
        var container = ctx?.Container;
        if (container == null)
            throw new Exception("Inject went wrong");
        _states = new()
        {
            ["Idle"] = container.Resolve<IdleState>(),
            ["Search"] = container.Resolve<SearchState>(),
            ["Collect"] = container.Resolve<CollectState>()
        };
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_states == null)
            InjectAll();
        SetCurrentStateFrom(stateInfo);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => _currentState?.Update();
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => _currentState?.Exit();

    private void SetCurrentStateFrom(AnimatorStateInfo stateInfo)
    {
        var newState = GetState(stateInfo);
        if (newState == null || newState == _currentState)
            return;
        _currentState = newState;
        _currentState.Enter();
    }

    private IState GetState(AnimatorStateInfo stateInfo)
    {
        foreach ((var name, var state) in _states)
            if (stateInfo.IsName(name))
                return state;
        return null;
    }
}
 