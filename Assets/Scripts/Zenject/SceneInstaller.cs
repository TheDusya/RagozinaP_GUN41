using Assets.Scripts;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField]
    Canvas _restartCanvas;
    [SerializeField]
    GameObject _progressBar;
    [SerializeField]
    GameObject divider;
    private Battlefield _battlefield;
    Controls _controls;
    public override void InstallBindings()
    {
        _controls = new Controls();
        FillCanvas();
        Container.BindInstance(_controls.Game.Get()).AsSingle();
        _battlefield = gameObject.GetOrAddComponent<Battlefield>();
        _battlefield.SetDivider(divider);
        SharedDataManager sharedData = new(); 
        Container.BindInstance(sharedData).AsSingle();
        Container.BindInstance(_battlefield).AsSingle();
        Container.BindInstance(_restartCanvas).AsSingle();
        Container.BindInstance(GetProgressImage()).AsSingle();
    }
    private void FillCanvas()
    {
        _restartCanvas = _restartCanvas != null ? _restartCanvas : FindObjectOfType<Canvas>();
        if (_restartCanvas == null)
            throw new Exception("Restart canvas not found!");
    }
    private Image GetProgressImage()
    {
        if (_progressBar == null ||
            !(_progressBar.GetComponent<Image>() is Image image && image.type is Image.Type.Filled))
            throw new Exception("Invalid progress bar!");
        return image;
    }
    private void OnDestroy() => _controls.Dispose();
}