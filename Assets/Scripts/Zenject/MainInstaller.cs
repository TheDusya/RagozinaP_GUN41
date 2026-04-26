using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        if (gameObject.TryGetComponent<SceneController>(out var sceneController))
            Container.Bind<SceneController>().FromInstance(sceneController).AsSingle();
        else
            Debug.LogError("Scene controller component not found!");
    }
}