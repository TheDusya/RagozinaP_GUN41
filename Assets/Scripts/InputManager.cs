using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputManager : MonoBehaviour
{
    [Inject(Id = "Game")]
    InputActionMap actionMap;
    private InputAction _restart;
    private void OnEnable()
    {
        if (actionMap == null)
        {
            Debug.LogError("No action map! Something is very wrong!");
            return;
        }
        actionMap.Enable();
        _restart = actionMap.actions.FirstOrDefault(action => action.name == "Restart");
        if (_restart == null)
            Debug.LogError("Input action for Restart not found!");
    }
    private void Update()
    {
        
    }
    private void OnDisable() => actionMap?.Disable();
}
