using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    Canvas _restartCanvas;
    [SerializeField]
    GameObject _progressBar;
    [SerializeField]
    CellPaletteSettings _cellPalette;
    private CellManager _cellManager;
    Controls _controls;
    public override void InstallBindings()
    {
        if (gameObject.TryGetComponent<SceneController>(out var sceneController))
            Container.Bind<SceneController>().FromInstance(sceneController).AsSingle();
        else
            Debug.LogError("Scene controller component not found!");
        _controls = new Controls();
        FillCanvas();
        Container.BindInstance(_controls.Game.Get()).WithId("Game").AsSingle();
        _cellManager = gameObject.GetOrAddComponent<CellManager>();
        Container.BindInstance(_cellManager).WithId("CellManager").AsSingle();
        Container.BindInstance(_restartCanvas).WithId("RestartCanvas").AsSingle();
        Container.BindInstance(GetProgressImage()).WithId("ProgressBar").AsSingle();
        Container.BindInstance(_cellPalette).WithId("CellPalette").AsSingle();
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