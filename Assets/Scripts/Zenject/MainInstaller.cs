using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    Canvas _restartCanvas;
    [SerializeField]
    GameObject _progressBar;
    [SerializeField]
    CellPaletteSettings _cellPalette;
    public override void InstallBindings()
    {
        if (gameObject.TryGetComponent<SceneController>(out var sceneController))
            Container.Bind<SceneController>().FromInstance(sceneController).AsSingle();
        else
            Debug.LogError("Scene controller component not found!");
        FillCanvas();
        Container.BindInstance(_restartCanvas).WithId("RestartCanvas").AsSingle();
        Container.BindInstance(GetProgressImage()).WithId("ProgressBar").AsSingle();
        Container.BindInstance(_cellPalette).AsSingle();

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
}