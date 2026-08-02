using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    GameParameters _gameParameters;
    public override void InstallBindings()
    {
        Container.Bind<GameParameters>().FromInstance(_gameParameters).AsSingle();
    }
}