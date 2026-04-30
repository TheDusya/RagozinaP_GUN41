using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class InputManager : MonoBehaviour
{
    [Inject(Id = "Game")]
    InputActionMap _actionMap;
    [Inject(Id = "RestartCanvas")]
    Canvas _canvas;
    [Inject(Id = "ProgressBar")]
    Image _progressBar;
    [Inject]
    SceneController _sceneController;

    [SerializeField, Min(10f)]
    float _fillingSpeed = 100f;

    private InputAction _restart;
    private void OnEnable()
    {
        CheckEverything();
        _actionMap.Enable();
        _restart = _actionMap.actions.FirstOrDefault(action => action.name == "Restart");
        if (_restart == null)
            throw new Exception("No action for restart! Something is very, very wrong!");
        _progressBar.fillAmount = 0;
    }
    private void CheckEverything()
    {
        if (_actionMap == null)
            throw new Exception("No action map! Something is very wrong!");
        if (_canvas == null)
            throw new Exception("No restart canvas found");
        if (_progressBar == null)
            throw new Exception("No progress bar found");
        if (_sceneController == null)
            throw new Exception("No scene controller found");

    }
    private void Update()
    {
        if (_restart.IsInProgress()) //!
        {
            _canvas.enabled = true;
            _canvas.gameObject.SetActive(true);
            var progress = _progressBar.fillAmount + (1 / _fillingSpeed);
            if (progress < 1)
                _progressBar.fillAmount = progress;
            else
            {
                _sceneController.ReloadGameScene();
                _progressBar.fillAmount = 0;
                _canvas.enabled = false;
            }
        }
        else
        { 
            _canvas.enabled = false;
            _progressBar.fillAmount = 0;
        }

    }
    private void OnDisable() 
    {
        _actionMap?.Disable();
    }
}
