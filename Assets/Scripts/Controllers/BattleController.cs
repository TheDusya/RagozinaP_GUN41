using Assets.Scripts;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

public class BattleController : MonoBehaviour
{
    [Inject]
    InputActionMap _actionMap;
    [Inject(Id = "RestartCanvas")]
    Canvas _canvas;
    [Inject(Id = "ProgressBar")]
    Image _progressBar;
    [Inject]
    SharedDataManager _dataManager;

    [SerializeField, Min(10f)]
    float _fillingSpeed = 100f;

    private InputAction _restart;
    private InputAction _confirm;
    private InputAction _cancel;
    private void OnEnable()
    {
        CheckEverything();
        _actionMap.Enable();
        _restart = _actionMap.actions.FirstOrDefault(action => action.name == "Restart");
        _confirm = _actionMap.actions.FirstOrDefault(action => action.name == "Confirm");
        _cancel = _actionMap.actions.FirstOrDefault(action => action.name == "Cancel");
        _cancel.started += Cancel;
        _confirm.started += Confirm;
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
    }
    private void Update()
    {
        CheckRestart();
    }

    private void CheckRestart()
    {
        if (_restart.IsInProgress())
        {
            _canvas.enabled = true;
            _canvas.gameObject.SetActive(true);
            var progress = _progressBar.fillAmount + (1 / _fillingSpeed);
            if (progress < 1)
                _progressBar.fillAmount = progress;
            else
            {
                SceneManager.LoadScene(0);
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

    private void Cancel(CallbackContext ctx)
    {
    }
    private void Confirm(CallbackContext ctx)
    {
        if (_dataManager.CurrentState == State.WaitingForConfirm)
            _dataManager.Confirm();
    }

    private void OnDisable() 
    {
        _actionMap?.Disable();
    }
    private void OnDestroy()
    {
        _cancel.started -= Cancel;
        _confirm.started -= Confirm;
    }
}
