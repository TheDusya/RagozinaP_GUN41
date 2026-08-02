using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    GameParameters _gameParameters;
    public override void InstallBindings()
    {
        if (_gameParameters != null)
            Container.Bind<GameParameters>().FromInstance(_gameParameters).AsSingle();
        else 
            throw new System.Exception("No GameParameters found!");
    }
}