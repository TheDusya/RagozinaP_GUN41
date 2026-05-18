using Assets.Scripts;
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
    [Inject]
    Canvas _canvas;
    [Inject]
    Image _progressBar;
    [Inject]
    SharedDataManager _dataManager;

    [SerializeField, Min(1f)]
    float _fillingSpeed = 1f;

    private InputAction _restart;
    private InputAction _confirm;
    private InputAction _cancel;
    private void OnEnable()
    {
        _actionMap.Enable();
        _restart = _actionMap.actions.FirstOrDefault(action => action.name == "Restart");
        _confirm = _actionMap.actions.FirstOrDefault(action => action.name == "Confirm");
        _cancel = _actionMap.actions.FirstOrDefault(action => action.name == "Cancel");
        _cancel.started += Cancel;
        _confirm.started += Confirm;
        _restart.started += Restart;
        _restart.canceled += RestartOver;
        _progressBar.fillAmount = 0;
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
            var progress = _progressBar.fillAmount + _fillingSpeed * Time.deltaTime;
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

    private void Cancel(CallbackContext ctx) => _dataManager.Cancel();
    private void Confirm(CallbackContext ctx) => _dataManager.Confirm();

    private void Restart(CallbackContext ctx) => _dataManager.Restart();
    private void RestartOver(CallbackContext ctx) => _dataManager.RestartOver();

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
